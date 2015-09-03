// RTMClient
// RTMClient.Camera.Module
// SettingsPageViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Devices.Enumeration;
using RTMClient.Camera.Module.Annotations;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.Module.Settings
{
    public class SettingsPageViewModel : ISettingsPageViewModel, INotifyPropertyChanged
    {
        private readonly IModuleConfiguration configuration;

        private readonly IList<string> videoSizes = new List<string>();
        private bool hightQualityVideo;
        private bool lowQualityVideo;
        private bool mediumQualityVideo = true;

        public SettingsPageViewModel(IModuleConfiguration moduleConfiguration)
        {
            configuration = moduleConfiguration;
            configuration.StreamingValueChanged += OnStreamingValueChanged;
            configuration.CurrentCameraChanged += OnCurrentCameraChanged;
            configuration.SupportedVideoSizesChanged += OnSupportedVideoSizesChanged;
            configuration.CurrentVideoSizeChanged += OnCurrentVideoSizeChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<SettingsValidationArgs> PortValidation;

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

        public IList<string> VideoSizes
        {
            get
            {
                videoSizes.Clear();
                foreach (
                    var size in
                        configuration.SupportedVideoSizes.Select(videoSize => $"{videoSize.Width}x{videoSize.Height}"))
                {
                    videoSizes.Add(size);
                }
                return videoSizes;
            }
        }

        public int CurrentVideoSize
        {
            get { return configuration.CurrentVideoSizeIndex; }
            set { configuration.CurrentVideoSizeIndex = value; }
        }

        public bool HighQualityVideo
        {
            get { return configuration.VideoQuality > 0.5; }
            set
            {
                hightQualityVideo = value;

                if (!hightQualityVideo)
                {
                    return;
                }

                configuration.VideoQuality = 0.75f;
                MediumQualityVideo = false;
                LowQualityVideo = false;
            }
        }

        public bool MediumQualityVideo
        {
            get { return Math.Abs(configuration.VideoQuality - 0.5) < 0.001; }
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
            get { return configuration.VideoQuality < 0.5; }
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
                OnPropertyChanged(nameof(Port));

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

        private void OnStreamingValueChanged(object sender, bool e)
        {
            OnPropertyChanged(nameof(Streaming));
        }

        private void OnCurrentCameraChanged(object sender, Panel e)
        {
            OnPropertyChanged(nameof(FrontCameraOn));
            OnPropertyChanged(nameof(BackCameraOn));
        }

        private void OnCurrentVideoSizeChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CurrentVideoSize));
        }

        private void OnSupportedVideoSizesChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(VideoSizes));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}