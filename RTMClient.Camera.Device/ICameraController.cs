// RTMClient
// RTMClient.Camera.Device
// ICameraController.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.Media.Capture;

namespace RTMClient.Camera.Device
{
    public interface ICameraController
    {
        MediaCapture Source { get; }

        Task InitializeAsync();
        Task StartAsync();
        Task RotateVideoAsync();
        void SetDisplayOrientationAsync(DisplayOrientations orientation);
        Task<VideoFrame> GetFrameAsync(VideoFrame videoFrame);
        Task StopAsync();
        Task DeinitializeAsync();
    }
}