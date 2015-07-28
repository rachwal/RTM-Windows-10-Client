// RTMClient
// RTMClient.Camera.Module.Configuration
// IModuleConfiguration.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;

namespace RTMClient.Camera.Module.Configuration
{
    public interface IModuleConfiguration
    {
        bool Streaming { get; set; }
        event EventHandler<bool> StreamingValueChanged;

        Panel CurrentPanel { get; set; }
        event EventHandler<Panel> CurrentCameraChanged;

        IList<VideoEncodingProperties> SupportedVideoSizes { get; set; }
        event EventHandler SupportedVideoSizesChanged;

        int CurrentVideoSizeIndex { get; set; }
        VideoEncodingProperties CurrentVideoSize { get; }
        event EventHandler CurrentVideoSizeChanged;

        Uri HostAddress { get; set; }
        float VideoQuality { get; set; }

        event EventHandler VideoQualityChanged;
    }
}