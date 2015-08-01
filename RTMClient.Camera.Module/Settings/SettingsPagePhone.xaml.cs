// RTMClient
// RTMClient.Camera.Module
// SettingsPagePhone.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace RTMClient.Camera.Module.Settings
{
    public sealed partial class SettingsPagePhone : ISettingsPage
    {
        public SettingsPagePhone(ISettingsPageViewModel viewModel)
        {
            InitializeComponent();
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += OnBackPressed;
            }
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
        }
    }
}