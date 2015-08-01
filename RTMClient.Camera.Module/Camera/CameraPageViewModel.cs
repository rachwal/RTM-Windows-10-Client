// RTMClient
// RTMClient.Camera.Module
// CameraPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using RTMClient.Camera.Device;
using RTMClient.Camera.Module.Annotations;
using RTMClient.Camera.Module.Commands;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.Module.Camera
{
    public class CameraPageViewModel : ICameraPageViewModel, INotifyPropertyChanged
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

        private readonly DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
        private readonly DisplayRequest displayRequest = new DisplayRequest();

        public CameraPageViewModel(ICameraController cameraController, IModuleConfiguration moduleConfiguration,
            ICommands moduleCommands)
        {
            StartStreamingButtonVisibility = Visibility.Visible;
            StopStreamingButtonVisibility = Visibility.Collapsed;

            controller = cameraController;
            configuration = moduleConfiguration;
            commands = moduleCommands;

            configuration.StreamingValueChanged += OnStreamingChanged;

            Application.Current.Resuming += OnResuming;
            Application.Current.Suspending += OnSuspending;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            StopCamera();
        }

        private void OnResuming(object sender, object e)
        {
            StartCamera();
        }

        private async void DisplayInformationOrientationChanged(DisplayInformation sender, object args)
        {
            controller.SetDisplayOrientationAsync(displayInformation.CurrentOrientation);
            await controller.RotateVideoAsync();
        }

        private async Task StartCameraAsync()
        {
            await controller.InitializeAsync();

            displayRequest.RequestActive();
            CaptureElement.Source = controller.Source;
            OnPropertyChanged(nameof(CaptureElement));
            await controller.StartAsync();
        }

        private async Task StopCameraAsync()
        {
            await controller.StopAsync();

            CaptureElement.Source = null;
            OnPropertyChanged(nameof(CaptureElement));
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

            OnPropertyChanged(nameof(StartStreamingButtonVisibility));
            OnPropertyChanged(nameof(StopStreamingButtonVisibility));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void StartCamera()
        {
            controller.SetDisplayOrientationAsync(displayInformation.CurrentOrientation);

            displayInformation.OrientationChanged += DisplayInformationOrientationChanged;

            await StartCameraAsync();
        }

        public async void StopCamera()
        {
            await StopCameraAsync();

            displayInformation.OrientationChanged -= DisplayInformationOrientationChanged;
        }
    }
}