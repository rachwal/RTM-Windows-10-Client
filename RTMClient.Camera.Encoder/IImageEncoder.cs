// RTMClient
// RTMClient.Camera.Encoder
// IImageEncoder.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System.Threading.Tasks;
using Windows.Media;

namespace RTMClient.Camera.Encoder
{
    public interface IImageEncoder
    {
        Task<string> Encode(VideoFrame frame);
    }
}