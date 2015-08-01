// RTMClient
// RTMClient.Camera.Module
// ICameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RTMClient.Camera.Module.Camera
{
    public interface ICameraPageViewModel
    {
        CaptureElement CaptureElement { get; }
        ICommand ChangeCameraCommand { get; }
        ICommand OpenSettingsCommand { get; }
        ICommand StartStreamingCommand { get; }
        ICommand StopStreamingCommand { get; }
        ICommand ShowAboutCommand { get; }
        Visibility StartStreamingButtonVisibility { get; set; }
        Visibility StopStreamingButtonVisibility { get; set; }
        void StartCamera();
        void StopCamera();
    }
}