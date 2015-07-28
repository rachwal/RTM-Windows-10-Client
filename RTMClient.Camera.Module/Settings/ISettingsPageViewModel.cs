// RTMClient
// RTMClient.Camera.Module
// ISettingsPageViewModel.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;

namespace RTMClient.Camera.Module.Settings
{
    public interface ISettingsPageViewModel
    {
        bool Streaming { get; set; }
        bool FrontCameraOn { get; set; }
        bool BackCameraOn { get; set; }
        IList<string> VideoSizes { get; }
        int CurrentVideoSize { get; set; }
        bool HighQualityVideo { get; set; }
        bool MediumQualityVideo { get; set; }
        bool LowQualityVideo { get; set; }
        string Host { get; set; }
        int Port { get; set; }

        event EventHandler<SettingsValidationArgs> PortValidation;
    }
}