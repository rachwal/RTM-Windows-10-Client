// RTMClient
// RTMClient.Camera.Module.Configuration
// IModuleConfiguration.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;

namespace RTMClient.Camera.Module.Configuration
{
    public interface IModuleConfiguration
    {
        bool Streaming { get; set; }

        Panel CurrentPanel { get; set; }

        IList<VideoEncodingProperties> SupportedVideoSizes { get; set; }

        int CurrentVideoSizeIndex { get; set; }
        VideoEncodingProperties CurrentVideoSize { get; }

        Uri HostAddress { get; set; }
        float VideoQuality { get; set; }
        event EventHandler<bool> StreamingValueChanged;
        event EventHandler<Panel> CurrentCameraChanged;
        event EventHandler SupportedVideoSizesChanged;
        event EventHandler CurrentVideoSizeChanged;

        event EventHandler VideoQualityChanged;

        void UpdateSupportedVideoSizes(IReadOnlyList<IMediaEncodingProperties> properties);
    }
}