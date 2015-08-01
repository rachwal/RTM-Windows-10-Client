// RTMClient
// RTMClient.Camera.Module
// ICommands.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Input;

namespace RTMClient.Camera.Module.Commands
{
    public interface ICommands
    {
        ICommand ShowAbout();
        ICommand ChangeCamera();
        ICommand OpenSettings();
        ICommand StartStreaming();
        ICommand StopStreaming();
    }
}