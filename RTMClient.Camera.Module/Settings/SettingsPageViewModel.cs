// RTMClient
// RTMClient.Camera.Module
// SettingsPageViewModel.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Enumeration;
using Microsoft.Practices.Prism.Mvvm;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.Module.Settings
{
    public class SettingsPageViewModel : ViewModel, ISettingsPageViewModel
    {
        private readonly IModuleConfiguration configuration;

        public event EventHandler<SettingsValidationArgs> PortValidation;

        private bool hightQualityVideo;
        private bool mediumQualityVideo = true;
        private bool lowQualityVideo;

        public SettingsPageViewModel(IModuleConfiguration moduleConfiguration)
        {
            configuration = moduleConfiguration;

            configuration.StreamingValueChanged += OnStreamingValueChanged;
            configuration.CurrentCameraChanged += OnCurrentCameraChanged;
            configuration.SupportedVideoSizesChanged += OnSupportedVideoSizesChanged;
        }

        public bool Streaming
        {
            get { return configuration.Streaming; }
            set { configuration.Streaming = value; }
        }

        public bool FrontCameraOn
        {
            get { return configuration.CurrentPanel == Panel.Front; }
            set { configuration.CurrentPanel = value ? Panel.Front : Panel.Back; }
        }

        public bool BackCameraOn
        {
            get { return configuration.CurrentPanel == Panel.Back; }
            set { configuration.CurrentPanel = value ? Panel.Back : Panel.Front; }
        }

        public IList<string> VideoSizes { get; } = new List<string>();

        public int CurrentVideoSize
        {
            get { return configuration.CurrentVideoSizeIndex; }
            set { configuration.CurrentVideoSizeIndex = value; }
        }

        public bool HighQualityVideo
        {
            get { return hightQualityVideo; }
            set
            {
                hightQualityVideo = value;

                if (!hightQualityVideo)
                {
                    return;
                }

                configuration.VideoQuality = 0.5f;
                MediumQualityVideo = false;
                LowQualityVideo = false;
            }
        }
        
        public bool MediumQualityVideo
        {
            get { return mediumQualityVideo; }
            set
            {
                mediumQualityVideo = value;

                if (!mediumQualityVideo)
                {
                    return;
                }

                configuration.VideoQuality = 0.5f;
                HighQualityVideo = false;
                LowQualityVideo = false;
            }
        }
        
        public bool LowQualityVideo
        {
            get { return lowQualityVideo; }
            set
            {
                lowQualityVideo = value;

                if (!lowQualityVideo)
                {
                    return;
                }

                configuration.VideoQuality = 0.25f;
                HighQualityVideo = false;
                MediumQualityVideo = false;
            }
        }

        public string Host
        {
            get { return configuration.HostAddress?.Host ?? "Enter Host Address"; }
            set { configuration.HostAddress = new Uri($"http://{value}:{Port}"); }
        }
        
        public int Port
        {
            get { return configuration.HostAddress?.Port ?? 9000; }
            set
            {
                OnPropertyChanged("Port");

                if (value < 0 || value > 65536)
                {
                    PortValidation?.Invoke(this,
                        new SettingsValidationArgs(false,
                            "Port value should be an integer greater than than zero up to a value of 65535."));
                }
                else
                {
                    configuration.HostAddress = new Uri($"http://{Host}:{value}");
                }
            }
        }

        private void OnCurrentCameraChanged(object sender, Panel e)
        {
            OnPropertyChanged("FrontCameraOn");
            OnPropertyChanged("BackCameraOn");
        }

        private void OnStreamingValueChanged(object sender, bool e)
        {
            OnPropertyChanged("Streaming");
        }

        private void OnSupportedVideoSizesChanged(object sender, EventArgs e)
        {
            VideoSizes.Clear();

            foreach (
                var size in
                    configuration.SupportedVideoSizes.Select(
                        videoSize => $"{videoSize.Width}x{videoSize.Height}"))
            {
                VideoSizes.Add(size);
            }

            OnPropertyChanged("VideoSizes");
        }
    }
}