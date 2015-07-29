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
        public event EventHandler<Panel> CurrentCameraChanged;
        public event EventHandler SupportedVideoSizesChanged;
        public event EventHandler CurrentVideoSizeChanged;
        public event EventHandler VideoQualityChanged;

        private bool streaming;
        private Panel currentPanel = Panel.Back;
        private IList<VideoEncodingProperties> supportedVideoSizes = new List<VideoEncodingProperties>();
        private int currentVideoSizeIndex;
        private Uri hostAddress;
        private float videoQuality = 0.5f;

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
    }
}