// RTMClient
// RTMClient
// App.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Microsoft.ApplicationInsights;
using Microsoft.Practices.Unity;
using RTMClient.Camera.Module;
using RTMClient.Camera.Module.Camera;
using RTMClient.Navigation;
using RTMClient.Register;

namespace RTMClient
{
    sealed partial class App
    {
        private static readonly IUnityContainer container = new UnityContainer();

        public App()
        {
            WindowsAppInitializer.InitializeAsync(
                WindowsCollectors.Metadata |
                WindowsCollectors.Session);
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IApplicationRegister, ApplicationRegister>(new ContainerControlledLifetimeManager());

            AddModules();

            container.Resolve<INavigationService>().Navigate<ICameraPage>();
            Window.Current.Activate();
            base.OnLaunched(e);
        }

        private void AddModules()
        {
            var register = container.Resolve<IApplicationRegister>();

            register.AddModule<CameraModule>();
        }
    }
}