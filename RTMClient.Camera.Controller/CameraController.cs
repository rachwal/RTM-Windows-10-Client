// RTMClient
// RTMClient.Camera.Device
// CameraController.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.Device
{
    public class CameraController : ICameraController
    {
        private readonly IModuleConfiguration configuration;

        public MediaCapture Source { get; private set; }

        public CameraController(IModuleConfiguration moduleConfiguration)
        {
            configuration = moduleConfiguration;

            configuration.CurrentVideoSizeChanged += OnCurrentVideoSizeChanged;
        }

        private bool initialized;
        private Panel CurrentPanel => configuration.CurrentPanel;

        public async Task InitializeAsync()
        {
            if (Source != null)
            {
                return;
            }

            var deviceInformation = await TryGetDeviceInformationFromPanel(CurrentPanel);
            if (deviceInformation == null)
            {
                return;
            }

            Source = new MediaCapture();

            try
            {
                await
                    Source.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = deviceInformation.Id });
                initialized = true;
            }
            catch
            {
                return;
            }

            var properties = Source.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview);
            var supportedProperties = new List<VideoEncodingProperties>();

            foreach (var encodingProperty in properties.Cast<VideoEncodingProperties>().Where(encodingProperty => !supportedProperties.Any(
                e => e.Width == encodingProperty.Width && e.Height == encodingProperty.Height)))
            {
                supportedProperties.Add(encodingProperty);
            }

            configuration.SupportedVideoSizes = supportedProperties;
        }

        private async Task<DeviceInformation> TryGetDeviceInformationFromPanel(Panel panel)
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var deviceOnPanel =
                devices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == panel);
            return deviceOnPanel ?? devices.FirstOrDefault();
        }

        private bool started;

        public async Task StartAsync()
        {
            if (!initialized || started)
            {
                return;
            }

            try
            {
                await Source.StartPreviewAsync();
                started = true;
            }
            catch
            {
                started = false;
            }

            await RotateVideoAsync();
        }

        private readonly Guid rotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");
        private DisplayOrientations displayOrientation = DisplayOrientations.Portrait;

        public async Task RotateVideoAsync()
        {
            var angle = GetAngle(displayOrientation);
            var mediaEncodingProperties =
                Source.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            mediaEncodingProperties.Properties.Add(rotationKey, angle);
            await Source.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, mediaEncodingProperties, null);
        }

        private int GetAngle(DisplayOrientations orientation)
        {
            switch (orientation)
            {
                case DisplayOrientations.Portrait:
                    return CurrentPanel == Panel.Front ? 270 : 90;
                case DisplayOrientations.LandscapeFlipped:
                    return 180;
                case DisplayOrientations.PortraitFlipped:
                    return CurrentPanel == Panel.Front ? 90 : 270;
                default:
                    return 0;
            }
        }

        public void SetDisplayOrientationAsync(DisplayOrientations orientation)
        {
            displayOrientation = orientation;
        }

        public async Task<VideoFrame> GetFrameAsync(VideoFrame videoFrame)
        {
            return await Source.GetPreviewFrameAsync(videoFrame);
        }

        public async Task StopAsync()
        {
            if (!initialized || !started)
            {
                return;
            }

            try
            {
                await Source.StopPreviewAsync();
            }
            catch
            {
            }

            started = false;
        }

        public async Task DeinitializeAsync()
        {
            if (!initialized)
            {
                return;
            }

            if (started)
            {
                await StopAsync();
            }

            if (Source != null)
            {
                initialized = false;
                Source.Dispose();
                Source = null;
            }
        }

        private async void OnCurrentVideoSizeChanged(object sender, EventArgs e)
        {
            var streaming = configuration.Streaming;
            configuration.Streaming = false;
            var videoSize = configuration.CurrentVideoSize;
            await Source.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, videoSize);
            configuration.Streaming = streaming;
        }
    }
}