// RTMClient
// RTMClient.Register
// ApplicationRegister.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Microsoft.Practices.Unity;
using RTMClient.Module;

namespace RTMClient.Register
{
    public class ApplicationRegister : IApplicationRegister
    {
        private readonly IUnityContainer container;

        public ApplicationRegister(IUnityContainer unityContainer)
        {
            container = unityContainer;
        }

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