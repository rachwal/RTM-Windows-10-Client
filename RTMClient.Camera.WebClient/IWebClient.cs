// RTMClient
// RTMClient.Camera.WebClient
// IWebClient.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Threading.Tasks;

namespace RTMClient.Camera.WebClient
{
    public interface IWebClient
    {
        Task PostImageAsync(string encodedImage, Action callback);
    }
}