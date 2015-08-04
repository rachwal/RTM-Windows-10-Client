// RTMClient
// RTMClient.Camera.Module
// CameraModule.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using RTMClient.Camera.Device;
using RTMClient.Camera.Encoder;
using RTMClient.Camera.Module.About;
using RTMClient.Camera.Module.Camera;
using RTMClient.Camera.Module.Commands;
using RTMClient.Camera.Module.Configuration;
using RTMClient.Camera.Module.Settings;
using RTMClient.Camera.StreamingService;
using RTMClient.Camera.WebClient;
using RTMClient.Module;

namespace RTMClient.Camera.Module
{
    public class CameraModule : IModule
    {
        private readonly IUnityContainer container;
        
        public CameraModule(IUnityContainer unityContainer)
        {
            container = unityContainer;
        }

        public void Initialize()
        {
            container.RegisterType<IStreamingService, VideoStreamingService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICameraController, CameraController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IModuleConfiguration, ModuleConfiguration>(new ContainerControlledLifetimeManager());

            container.RegisterType<IWebClient, RTMWebClient>(new ContainerControlledLifetimeManager());
            container.RegisterType<IImageEncoder, ImageEncoder>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICommands, CameraModuleCommands>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICameraPageViewModel, CameraPageViewModel>();
            container.RegisterType<ISettingsPageViewModel, SettingsPageViewModel>();

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                container.RegisterType<ISettingsPage, SettingsPagePhone>();
                container.RegisterType<IAboutPage, AboutPagePhone>();
                container.RegisterType<ICameraPage, CameraPagePhone>();
            }
            else
            {
                container.RegisterType<ISettingsPage, SettingsPage>();
                container.RegisterType<IAboutPage, AboutPage>();
                container.RegisterType<ICameraPage, CameraPage>();
            }

            container.Resolve<IStreamingService>();
        }
    }
}