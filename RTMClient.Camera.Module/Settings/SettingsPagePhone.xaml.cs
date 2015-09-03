// RTMClient
// RTMClient.Camera.Module
// SettingsPagePhone.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
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

            ViewModel = viewModel;
            ViewModel.PortValidation += ViewModelPortValidation;
        }

        private ISettingsPageViewModel ViewModel
        {
            get { return (ISettingsPageViewModel) DataContext; }
            set { DataContext = value; }
        }

        private void VideoSizesLoaded(object sender, RoutedEventArgs e)
        {
            var videoSizes = (ComboBox) sender;
            videoSizes.SelectedItem = ViewModel.VideoSizes[ViewModel.CurrentVideoSize];
            videoSizes.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private async void ViewModelPortValidation(object sender, SettingsValidationArgs e)
        {
            if (e.Valid)
            {
                return;
            }
            var message = new MessageDialog(e.Message);
            await message.ShowAsync();
        }

        private void HostTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            HideKeyboard(e);
        }

        private void PortTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            HideKeyboard(e);
        }

        private void HideKeyboard(KeyRoutedEventArgs e)
        {
            if (ViewModel.Streaming)
            {
                ViewModel.Streaming = false;
            }

            if (e.Key == VirtualKey.Enter)
            {
                InputPane.GetForCurrentView().TryHide();
            }
        }
    }
}