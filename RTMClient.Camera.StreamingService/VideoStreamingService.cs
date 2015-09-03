// RTMClient
// RTMClient.Camera.StreamingService
// VideoStreamingService.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using RTMClient.Camera.Device;
using RTMClient.Camera.Encoder;
using RTMClient.Camera.Module.Configuration;
using RTMClient.Camera.WebClient;

namespace RTMClient.Camera.StreamingService
{
    public class VideoStreamingService : IStreamingService
    {
        private readonly IWebClient client;
        private readonly IModuleConfiguration configuration;
        private readonly ICameraController controller;
        private readonly IImageEncoder encoder;
        private bool readyToPost = true;

        public VideoStreamingService(ICameraController cameraController, IImageEncoder imageEncoder,
            IModuleConfiguration moduleConfiguration, IWebClient webClient)
        {
            encoder = imageEncoder;
            SendImage += OnSendImage;

            client = webClient;
            controller = cameraController;
            configuration = moduleConfiguration;

            configuration.StreamingValueChanged += OnStreamingValueChanged;
            configuration.CurrentVideoSizeChanged += OnCurrentVideoSizeChanged;
        }

        public void Send()
        {
            if (!configuration.Streaming)
            {
                return;
            }
            SendImage?.Invoke(this, EventArgs.Empty);
        }

        private event EventHandler SendImage;

        private async void OnSendImage(object sender, EventArgs e)
        {
            var frame = await GetVideoFrame();
            var encodedImage = await encoder.Encode(frame);
            PostImage(encodedImage);
        }

        private async Task<VideoFrame> GetVideoFrame()
        {
            var frameProperties = configuration.CurrentVideoSize;

            var videoFrame = new VideoFrame(BitmapPixelFormat.Rgba16, (int) frameProperties.Width,
                (int) frameProperties.Height);

            try
            {
                return await controller.GetFrameAsync(videoFrame);
            }

            catch (Exception)
            {
                return videoFrame;
            }
        }

        private void PostImage(string encodedImage)
        {
            if (!readyToPost)
            {
                readyToPost = true;
                Send();
            }
            else
            {
                client.PostImageAsync(encodedImage, Send);
            }
        }

        private void OnCurrentVideoSizeChanged(object sender, EventArgs e)
        {
            readyToPost = false;
        }

        private void OnStreamingValueChanged(object sender, bool e)
        {
            if (!configuration.Streaming)
            {
                return;
            }
            Send();
        }
    }
}