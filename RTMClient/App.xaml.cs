// RTMClient
// RTMClient
// App.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.ApplicationInsights;
using RTMClient.Camera.Module;
using RTMClient.Camera.Module.Camera;

namespace RTMClient
{
    sealed partial class App
    {
        private readonly ApplicationRegister register = new ApplicationRegister();

        public App()
        {
            WindowsAppInitializer.InitializeAsync(
                WindowsCollectors.Metadata |
                WindowsCollectors.Session);
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            register.AddModule<CameraModule>();

            register.SetStartingPage<ICameraPage>();

            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
            }

            Window.Current.Activate();
        }
    }
}