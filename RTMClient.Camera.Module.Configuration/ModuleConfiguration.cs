// RTMClient
// RTMClient.Camera.Module.Configuration
// ModuleConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace RTMClient.Camera.Module.Configuration
{
    public class ModuleConfiguration : IModuleConfiguration
    {
        private Panel currentPanel = Panel.Back;
        private int currentVideoSizeIndex;
        private Uri hostAddress;

        private volatile bool streaming;
        private IList<VideoEncodingProperties> supportedVideoSizes = new List<VideoEncodingProperties>();
        private float videoQuality = 0.5f;
        public event EventHandler<bool> StreamingValueChanged;
        public event EventHandler<Panel> CurrentCameraChanged;
        public event EventHandler SupportedVideoSizesChanged;
        public event EventHandler CurrentVideoSizeChanged;
        public event EventHandler VideoQualityChanged;

        public bool Streaming
        {
            get { return streaming; }
            set
            {
                streaming = value;
                StreamingValueChanged?.Invoke(this, streaming);
            }
        }

        public Panel CurrentPanel
        {
            get { return currentPanel; }
            set
            {
                currentPanel = value;
                CurrentCameraChanged?.Invoke(this, currentPanel);
            }
        }

        public IList<VideoEncodingProperties> SupportedVideoSizes
        {
            get { return supportedVideoSizes; }
            set
            {
                supportedVideoSizes = value;
                SupportedVideoSizesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

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

        public float VideoQuality
        {
            get { return videoQuality; }
            set
            {
                videoQuality = value;
                VideoQualityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateSupportedVideoSizes(IReadOnlyList<IMediaEncodingProperties> properties)
        {
            var supportedProperties = new List<VideoEncodingProperties>();

            foreach (
                var encodingProperty in
                    properties.Cast<VideoEncodingProperties>().Where(encodingProperty => !supportedProperties.Any(
                        e => e.Width == encodingProperty.Width && e.Height == encodingProperty.Height)))
            {
                supportedProperties.Add(encodingProperty);
            }

            VideoEncodingProperties currentProperties = null;

            if (SupportedVideoSizes.Count > 0)
            {
                var oldIndex = CurrentVideoSizeIndex;
                var oldparam = SupportedVideoSizes[oldIndex];

                foreach (var propertiese in supportedProperties)
                {
                    if (propertiese.Width == oldparam.Width && propertiese.Height == oldparam.Height &&
                        propertiese.Bitrate == oldparam.Bitrate)
                    {
                        currentProperties = propertiese;
                    }
                }
            }

            SupportedVideoSizes = supportedProperties;
            CurrentVideoSizeIndex = currentProperties != null ? supportedProperties.IndexOf(currentProperties) : 0;
        }
    }
}