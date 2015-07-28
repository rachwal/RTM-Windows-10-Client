// RTMClient
// RTMClient.Camera.Module
// CameraModuleCommands.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System.Windows.Input;
using Windows.Devices.Enumeration;
using Microsoft.Practices.Prism.Commands;
using RTMClient.Camera.Module.About;
using RTMClient.Camera.Module.Configuration;
using RTMClient.Camera.Module.Settings;

namespace RTMClient.Camera.Module.Commands
{
    public class CameraModuleCommands : ICommands
    {
        private readonly IModuleConfiguration configuration;
        private readonly ISettingsPage settings;
        private readonly IAboutPage about;

        public CameraModuleCommands(IModuleConfiguration moduleConfiguration, ISettingsPage settingsPage,
            IAboutPage aboutPage)
        {
            configuration = moduleConfiguration;
            settings = settingsPage;
            about = aboutPage;
        }

        public ICommand ShowAbout()
        {
            return new DelegateCommand(() => { about.Show(); });
        }

        public ICommand ChangeCamera()
        {
            return new DelegateCommand(() =>
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
            return new DelegateCommand(() => { settings.Show(); });
        }

        public ICommand StartStreaming()
        {
            return new DelegateCommand(() => { configuration.Streaming = true; });
        }

        public ICommand StopStreaming()
        {
            return new DelegateCommand(() => { configuration.Streaming = false; });
        }
    }
}