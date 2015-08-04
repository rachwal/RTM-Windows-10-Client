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

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
        private readonly DisplayRequest displayRequest = new DisplayRequest();
        private bool shouldResume;
        private bool changingCamera;

        public CameraPageViewModel(ICameraController cameraController, IModuleConfiguration moduleConfiguration,
            ICommands moduleCommands)
        {
            controller = cameraController;
            configuration = moduleConfiguration;
            commands = moduleCommands;

            configuration.StreamingValueChanged += OnStreamingChanged;

            UpdateStreamingButtons(configuration.Streaming);

            Window.Current.VisibilityChanged += OnVisibilityChanged;
        }

        private async void OnCurrentCameraChanged(object sender, Windows.Devices.Enumeration.Panel e)
        {
            if (changingCamera)
            {
                return;
            }
            changingCamera = true;
            await StopCamera();
            await StartCamera();
            changingCamera = false;
        }

        public async Task StartCamera()
        {
            controller.SetDisplayOrientationAsync(displayInformation.CurrentOrientation);

            displayInformation.OrientationChanged += DisplayInformationOrientationChanged;

            await StartCameraAsync();

            configuration.CurrentCameraChanged += OnCurrentCameraChanged;
        }

        public async Task StopCamera()
        {
            configuration.CurrentCameraChanged -= OnCurrentCameraChanged;

            await StopCameraAsync();

            displayInformation.OrientationChanged -= DisplayInformationOrientationChanged;
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
            UpdateStreamingButtons(streaming);
        }

        private void UpdateStreamingButtons(bool streaming)
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

        private async void OnVisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            if (e.Visible == false)
            {
                shouldResume = true;
                await StopCamera();
            }
            else
            {
                if (!shouldResume)
                {
                    return;
                }
                await StartCamera();
                shouldResume = false;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}