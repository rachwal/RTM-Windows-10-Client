// RTMClient
// RTMClient.Camera.Module.Configuration
// ModuleConfiguration.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace RTMClient.Camera.Module.Configuration
{
    public class ModuleConfiguration : IModuleConfiguration
    {
        public event EventHandler<bool> StreamingValueChanged;

        private bool streaming;

        public bool Streaming
        {
            get { return streaming; }
            set
            {
                streaming = value;
                StreamingValueChanged?.Invoke(this, streaming);
            }
        }

        public event EventHandler<Panel> CurrentCameraChanged;
        private Panel currentPanel = Panel.Back;

        public Panel CurrentPanel
        {
            get { return currentPanel; }
            set
            {
                currentPanel = value;
                CurrentCameraChanged?.Invoke(this, currentPanel);
            }
        }

        private IList<VideoEncodingProperties> supportedVideoSizes = new List<VideoEncodingProperties>();

        public IList<VideoEncodingProperties> SupportedVideoSizes
        {
            get { return supportedVideoSizes; }
            set
            {
                supportedVideoSizes = value;
                SupportedVideoSizesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SupportedVideoSizesChanged;
        private int currentVideoSizeIndex;

        public int CurrentVideoSizeIndex
        {
            get { return currentVideoSizeIndex; }
            set
            {
                currentVideoSizeIndex = value;
                CurrentVideoSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public VideoEncodingProperties CurrentVideoSize => SupportedVideoSizes[CurrentVideoSizeIndex];
        public event EventHandler CurrentVideoSizeChanged;

        private Uri hostAddress;
        private float videoQuality = 0.5f;

        public Uri HostAddress
        {
            get
            {
                if (hostAddress != null)
                {
                    return hostAddress;
                }
                var host = ApplicationData.Current.LocalSettings.Values["HostAddress"];
                var port = ApplicationData.Current.LocalSettings.Values["Port"] ?? 9000;
                hostAddress = host != null ? new Uri($"http://{host}:{port}") : null;
                return hostAddress;
            }
            set
            {
                if (value != null)
                {
                    ApplicationData.Current.LocalSettings.Values["HostAddress"] = value.Host;
                    ApplicationData.Current.LocalSettings.Values["Port"] = value.Port;
                }
                hostAddress = value;
            }
        }

        public event EventHandler VideoQualityChanged;

        public float VideoQuality
        {
            get { return videoQuality; }
            set
            {
                videoQuality = value;
                VideoQualityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}