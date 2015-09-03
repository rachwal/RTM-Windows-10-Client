// RTMClient
// RTMClient.Camera.Module
// CameraPage.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

namespace RTMClient.Camera.Module.Camera
{
    public sealed partial class CameraPage : ICameraPage
    {
        public CameraPage(ICameraPageViewModel viewModel)
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