// RTMClient
// RTMClient.Camera.WebClient
// RTMWebClient.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using RTMClient.Camera.Module.Configuration;

namespace RTMClient.Camera.WebClient
{
    public class RTMWebClient : IWebClient
    {
        private readonly IModuleConfiguration configuration;

        private readonly HttpClient httpClient = new HttpClient();
        private readonly HttpMediaTypeHeaderValue contentType = new HttpMediaTypeHeaderValue("application/json");

        public RTMWebClient(IModuleConfiguration moduleConfiguration)
        {
            configuration = moduleConfiguration;
        }

        public async Task PostImageAsync(string encodedImage, Action callback)
        {
            await PostImage(encodedImage);
            callback.DynamicInvoke();
        }

        private async Task PostImage(string encodedImage)
        {
            if (string.IsNullOrEmpty(encodedImage))
            {
                await Task.FromResult<object>(null);
                return;
            }
            try
            {
                var jsonData = "{\"data\" : \"" + encodedImage + "\"}";
                var msg = new HttpRequestMessage(new HttpMethod("POST"),
                    new Uri(configuration.HostAddress, "/api/images"));
                msg.Content = new HttpStringContent(jsonData);
                msg.Content.Headers.ContentType = contentType;

                await httpClient.SendRequestAsync(msg).AsTask();
            }
            catch (Exception)
            {
                configuration.Streaming = false;
            }
        }
    }
}