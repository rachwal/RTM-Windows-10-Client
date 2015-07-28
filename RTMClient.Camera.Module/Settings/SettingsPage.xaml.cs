// RTMClient
// RTMClient.Camera.Module
// SettingsPage.xaml.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace RTMClient.Camera.Module.Settings
{
    public sealed partial class SettingsPage : ISettingsPage
    {
        private ISettingsPageViewModel ViewModel
        {
            get { return (ISettingsPageViewModel) DataContext; }
            set { DataContext = value; }
        }

        public SettingsPage(ISettingsPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            ViewModel.PortValidation += ViewModelPortValidation;
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

        private void SettingsPageOnBackClick(object sender, BackClickEventArgs e)
        {
            Hide();
            e.Handled = true;
        }

        private double contentWidth;

        private void SettingsLoaded(object sender, RoutedEventArgs e)
        {
            var flyout = (SettingsFlyout) sender;
            contentWidth = flyout.ActualWidth - flyout.Padding.Left - flyout.Padding.Right;
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            var control = (Grid) sender;
            control.Width = contentWidth;
        }

        private void VideoSizesLoaded(object sender, RoutedEventArgs e)
        {
            var control = (ComboBox) sender;
            control.SelectedItem = ViewModel.VideoSizes[ViewModel.CurrentVideoSize];
            control.Width = contentWidth;
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