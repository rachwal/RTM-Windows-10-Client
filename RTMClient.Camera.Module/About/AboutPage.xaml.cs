// RTMClient
// RTMClient.Camera.Module
// AboutPage.xaml.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace RTMClient.Camera.Module.About
{
    public sealed partial class AboutPage : IAboutPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void AboutPageOnBackClick(object sender, BackClickEventArgs e)
        {
            Hide();
            e.Handled = true;
        }
    }
}