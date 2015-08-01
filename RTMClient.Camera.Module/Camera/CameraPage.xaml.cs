// RTMClient
// RTMClient.Camera.Module
// CameraPage.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace RTMClient.Camera.Module.Camera
{
    public sealed partial class CameraPage : ICameraPage
    {
        private ICameraPageViewModel ViewModel
        {
            get { return (ICameraPageViewModel) DataContext; }
            set { DataContext = value; }
        }

        public CameraPage(ICameraPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.StartCamera();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.StopCamera();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.StartCamera();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.StopCamera();
        }
    }
}