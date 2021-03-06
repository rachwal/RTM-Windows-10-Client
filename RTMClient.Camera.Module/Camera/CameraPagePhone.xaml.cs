﻿// RTMClient
// RTMClient.Camera.Module
// CameraPagePhone.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTMClient.Camera.Module.Camera
{
    public sealed partial class CameraPagePhone : ICameraPage
    {
        public CameraPagePhone(ICameraPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private ICameraPageViewModel ViewModel
        {
            get { return (ICameraPageViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void OnUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StopCamera();
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.StartCamera();
        }
    }
}