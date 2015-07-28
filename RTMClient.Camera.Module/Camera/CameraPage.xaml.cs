// RTMClient
// RTMClient.Camera.Module
// CameraPage.xaml.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using Microsoft.Practices.Prism.Mvvm;

namespace RTMClient.Camera.Module.Camera
{
    public sealed partial class CameraPage
    {
        public CameraPage()
        {
            InitializeComponent();
            ViewModelLocationProvider.AutoWireViewModelChanged(this);
        }
    }
}