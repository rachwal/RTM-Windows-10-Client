// RTMClient
// RTMClient.Navigation
// NavigationService.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Unity;

namespace RTMClient.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IUnityContainer container;
        private readonly List<Type> history = new List<Type>();

        public NavigationService(IUnityContainer unityContainer)
        {
            container = unityContainer;

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += OnBackPressed;
            }
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            GoBack();
        }

        public void Navigate<T>()
        {
            var page = container.Resolve<T>() as UIElement;
            if (page == null)
            {
                return;
            }
            var flyout = page as SettingsFlyout;
            if (flyout != null)
            {
                flyout.ShowIndependent();
            }
            else
            {
                history.Add(typeof (T));
                Window.Current.Content = page;
            }
        }

        private void GoBack()
        {
            try
            {
                if (history.Count > 1)
                {
                    var page = (UIElement) container.Resolve(history[history.Count - 2]);
                    history.RemoveAt(history.Count - 1);
                    Window.Current.Content = page;
                }
                else
                {
                    Application.Current.Exit();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}