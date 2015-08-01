// RTMClient
// RTMClient.Camera.Module
// AboutPagePhone.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;

namespace RTMClient.Camera.Module.About
{
    public sealed partial class AboutPagePhone : IAboutPage
    {
        public AboutPagePhone()
        {
            InitializeComponent();

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += OnBackPressed;
            }
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
        }
    }
}