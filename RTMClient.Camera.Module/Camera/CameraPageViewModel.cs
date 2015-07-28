// RTMClient
// RTMClient.Camera.Module
// CameraPageViewModel.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Graphics.Display;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using RTMClient.Camera.Device;
using RTMClient.Camera.Module.Commands;
using RTMClient.Camera.Module.Configuration;
using Panel = Windows.Devices.Enumeration.Panel;

namespace RTMClient.Camera.Module.Camera
{
    public class CameraPageViewModel : ViewModel
    {
        private readonly ICameraController controller;
        private readonly IModuleConfiguration configuration;
        private readonly ICommands commands;

        public CaptureElement CaptureElement { get; } = new CaptureElement {Stretch = Stretch.UniformToFill};
        public ICommand ChangeCameraCommand => commands.ChangeCamera();
        public ICommand OpenSettingsCommand => commands.OpenSettings();
        public ICommand StartStreamingCommand => commands.StartStreaming();
        public ICommand StopStreamingCommand => commands.StopStreaming();
        public ICommand ShowAboutCommand => commands.ShowAbout();
        public Visibility StartStreamingButtonVisibility { get; set; }
        public Visibility StopStreamingButtonVisibility { get; set; }

        public CameraPageViewModel(ICameraController cameraController, IModuleConfiguration moduleConfiguration,
            ICommands moduleCommands)
        {
            StartStreamingButtonVisibility = Visibility.Visible;
            StopStreamingButtonVisibility = Visibility.Collapsed;

            controller = cameraController;
            configuration = moduleConfiguration;
            commands = moduleCommands;

            configuration.CurrentCameraChanged += OnCurrentCameraChanged;
            configuration.StreamingValueChanged += OnStreamingChanged;
        }

        private readonly DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();

        public override async void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);

            await StopCameraAsync();

            displayInformation.OrientationChanged -= DisplayInformationOrientationChanged;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode,
            Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            controller.SetDisplayOrientationAsync(displayInformation.CurrentOrientation);

            displayInformation.OrientationChanged += DisplayInformationOrientationChanged;

            await StartCameraAsync();
        }

        private async void DisplayInformationOrientationChanged(DisplayInformation sender, object args)
        {
            controller.SetDisplayOrientationAsync(displayInformation.CurrentOrientation);
            await controller.RotateVideoAsync();
        }

        private readonly DisplayRequest displayRequest = new DisplayRequest();

        private async Task StartCameraAsync()
        {
            await controller.InitializeAsync();

            displayRequest.RequestActive();
            CaptureElement.Source = controller.Source;

            await controller.StartAsync();
        }

        private async Task StopCameraAsync()
        {
            await controller.StopAsync();

            CaptureElement.Source = null;
            displayRequest.RequestRelease();

            await controller.DeinitializeAsync();
        }

        private void OnStreamingChanged(object sender, bool streaming)
        {
            if (streaming)
            {
                StartStreamingButtonVisibility = Visibility.Collapsed;
                StopStreamingButtonVisibility = Visibility.Visible;
            }
            else
            {
                StartStreamingButtonVisibility = Visibility.Visible;
                StopStreamingButtonVisibility = Visibility.Collapsed;
            }

            OnPropertyChanged("StartStreamingButtonVisibility");
            OnPropertyChanged("StopStreamingButtonVisibility");
        }

        private bool restartingCamera;

        private async void OnCurrentCameraChanged(object sender, Panel e)
        {
            if (restartingCamera)
            {
                return;
            }
            restartingCamera = true;
            var streaming = configuration.Streaming;
            await StopCameraAsync();
            configuration.Streaming = false;
            await StartCameraAsync();
            configuration.Streaming = streaming;
            restartingCamera = false;
        }
    }
}