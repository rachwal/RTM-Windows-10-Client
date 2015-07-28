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

        public const string MainPage = "CameraPage";

        public Dictionary<string, Type> ViewTypes { get; }
        public Dictionary<Type, Type> ViewModelTypes { get; }

        public CameraModule(IUnityContainer unityContainer)
        {
            ViewTypes = new Dictionary<string, Type>();
            ViewModelTypes = new Dictionary<Type, Type>();

            container = unityContainer;
        }

        public void Initialize()
        {
            Register();
            Resolve();
        }

        private void Register()
        {
            RegisterTypes();

            RegisterViews();

            RegisterViewModels();
        }

        private void RegisterTypes()
        {
            container.RegisterType<IStreamingService, VideoStreamingService>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICameraController, CameraController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IModuleConfiguration, ModuleConfiguration>(new ContainerControlledLifetimeManager());
            container.RegisterType<IWebClient, RTMWebClient>(new ContainerControlledLifetimeManager());

            container.RegisterType<IImageEncoder, ImageEncoder>();

            container.RegisterType<ICommands, CameraModuleCommands>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISettingsPageViewModel, SettingsPageViewModel>();
            container.RegisterType<ISettingsPage, SettingsPage>();
            container.RegisterType<IAboutPage, AboutPage>();

            container.RegisterType<CameraPageViewModel>();
        }

        private void RegisterViews()
        {
            ViewTypes.Add("CameraPage", typeof (CameraPage));
        }

        private void RegisterViewModels()
        {
            ViewModelTypes.Add(typeof (CameraPage), typeof (CameraPageViewModel));
        }

        private void Resolve()
        {
            container.Resolve<IStreamingService>();
        }
    }
}