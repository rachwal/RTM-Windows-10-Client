// RTMClient
// RTMClient.Camera.Module
// SettingsValidationArgs.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;

namespace RTMClient.Camera.Module.Settings
{
    public class SettingsValidationArgs : EventArgs
    {
        public readonly bool Valid;
        public readonly string Message;

        public SettingsValidationArgs(bool valid, string message = "")
        {
            Valid = valid;
            Message = message;
        }
    }
}