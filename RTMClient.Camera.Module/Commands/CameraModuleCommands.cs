// RTMClient
// RTMClient.Camera.Module
// CameraModuleCommands.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Windows.Input;
using Windows.Devices.Enumeration;
using RTMClient.Camera.Module.About;
using RTMClient.Camera.Module.Configuration;
using RTMClient.Camera.Module.Settings;
using RTMClient.Navigation;

namespace RTMClient.Camera.Module.Commands
{
    public class CameraModuleCommands : ICommands
    {
        private readonly IModuleConfiguration configuration;
        private readonly INavigationService service;

        public CameraModuleCommands(IModuleConfiguration moduleConfiguration, INavigationService navigationService)
        {
            service = navigationService;
            configuration = moduleConfiguration;
        }

        public ICommand ShowAbout()
        {
            return new CameraModuleCommand(() => { service.Navigate<IAboutPage>(); });
        }

        public ICommand ChangeCamera()
        {
            return new CameraModuleCommand(() =>
            {
                switch (configuration.CurrentPanel)
                {
                    case Panel.Back:
                        configuration.CurrentPanel = Panel.Front;
                        break;
                    case Panel.Front:
                        configuration.CurrentPanel = Panel.Back;
                        break;
                }
            });
        }

        public ICommand OpenSettings()
        {
            return new CameraModuleCommand(() => { service.Navigate<ISettingsPage>(); });
        }

        public ICommand StartStreaming()
        {
            return new CameraModuleCommand(() => { configuration.Streaming = true; });
        }

        public ICommand StopStreaming()
        {
            return new CameraModuleCommand(() => { configuration.Streaming = false; });
        }
    }
}