// RTMClient
// RTMClient
// ApplicationRegister.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using RTMClient.Module;

namespace RTMClient
{
    public class ApplicationRegister
    {
        private readonly IUnityContainer container = new UnityContainer();
        private readonly Dictionary<string, Type> viewTypes = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Type> viewModelTypes = new Dictionary<Type, Type>();

        public string StartPage { get; set; }

        public void Initialize(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                return;
            }
            container.RegisterInstance(navigationService);
        }

        public void AddModule<T>() where T : IModule
        {
            var module = container.Resolve<T>() as IModule;
            if (module == null)
            {
                throw new Exception($"Module {typeof (T)} was not registered");
            }

            module.Initialize();

            foreach (var viewType in module.ViewTypes.Where(viewType => !viewTypes.ContainsKey(viewType.Key)))
            {
                viewTypes.Add(viewType.Key, viewType.Value);
            }

            foreach (
                var viewModelType in
                    module.ViewModelTypes.Where(viewModelType => !viewModelTypes.ContainsKey(viewModelType.Key)))
            {
                viewModelTypes.Add(viewModelType.Key, viewModelType.Value);
            }
        }

        public object Resolve(Type type)
        {
            return type == null ? null : container.Resolve(type);
        }

        public Type GetPageType(string pageToken)
        {
            return viewTypes.ContainsKey(pageToken) ? viewTypes[pageToken] : null;
        }

        public Type GetModelViewType(Type viewType)
        {
            return viewModelTypes.ContainsKey(viewType) ? viewModelTypes[viewType] : null;
        }
    }
}