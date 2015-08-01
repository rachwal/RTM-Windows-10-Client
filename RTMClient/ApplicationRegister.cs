// RTMClient
// RTMClient
// ApplicationRegister.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;
using RTMClient.Module;

namespace RTMClient
{
    public class ApplicationRegister
    {
        private readonly IUnityContainer container = new UnityContainer();

        public void SetStartingPage<T>()
        {
            StartPage = container.Resolve<T>() as UIElement;
        }

        public UIElement StartPage { get; private set; }

        public void AddModule<T>() where T : IModule
        {
            var module = container.Resolve<T>() as IModule;
            if (module == null)
            {
                throw new Exception($"Module {typeof (T)} was not registered");
            }

            module.Initialize();
        }
    }
}