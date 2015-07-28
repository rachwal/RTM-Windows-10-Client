// RTMClient
// RTMClient
// App.xaml.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Prism.Mvvm;
using RTMClient.Camera.Module;

namespace RTMClient
{
    sealed partial class App
    {
        private readonly ApplicationRegister register = new ApplicationRegister();

        public App()
        {
            InitializeComponent();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            register.Initialize(NavigationService);

            register.AddModule<CameraModule>();

            register.StartPage = CameraModule.MainPage;

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                viewType => register.GetModelViewType(viewType));

            return base.OnInitializeAsync(args);
        }

        protected override Type GetPageType(string pageToken)
        {
            return register.GetPageType(pageToken);
        }

        protected override object Resolve(Type type)
        {
            return register.Resolve(type);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(register.StartPage, null);
            return Task.FromResult<object>(null);
        }
    }
}