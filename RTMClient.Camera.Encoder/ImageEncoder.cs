// RTMClient
// RTMClient.Camera.Encoder
// ImageEncoder.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage.Streams;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.Encoder
{
    public class ImageEncoder : IImageEncoder
    {
        private readonly IModuleConfiguration configuration;

        private BitmapPropertySet properties;

        public ImageEncoder(IModuleConfiguration moduleConfiguration)
        {
            configuration = moduleConfiguration;
            configuration.VideoQualityChanged += OnVideoQualityChanged;
            configuration.StreamingValueChanged += OnStreamingValueChanged;
        }

        public async Task<string> Encode(VideoFrame frame)
        {
            var array = await EncodeToBytes(frame);
            var encodedImage = Convert.ToBase64String(array);
            return encodedImage;
        }

        private async Task<byte[]> EncodeToBytes(VideoFrame frame)
        {
            byte[] array;

            using (var stream = new InMemoryRandomAccessStream())
            {
                var encoder = await CreateBitmapEncoder(stream);
                encoder.SetSoftwareBitmap(frame.SoftwareBitmap);
                await encoder.FlushAsync();
                array = new byte[stream.Size];
                await stream.ReadAsync(array.AsBuffer(), (uint) stream.Size, InputStreamOptions.None);
            }

            return array;
        }

        private async Task<BitmapEncoder> CreateBitmapEncoder(InMemoryRandomAccessStream stream)
        {
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream, properties);
            return encoder;
        }

        private BitmapPropertySet CreateBitmapPropertySet()
        {
            var propertySet = new BitmapPropertySet();
            var qualityValue = new BitmapTypedValue(configuration.VideoQuality, PropertyType.Single);
            propertySet.Add("ImageQuality", qualityValue);
            return propertySet;
        }

        private void OnStreamingValueChanged(object sender, bool e)
        {
            if (properties == null)
            {
                properties = CreateBitmapPropertySet();
            }
        }

        private void OnVideoQualityChanged(object sender, EventArgs e)
        {
            properties = CreateBitmapPropertySet();
        }
    }
}