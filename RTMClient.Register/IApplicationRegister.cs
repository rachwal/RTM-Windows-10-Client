// RTMClient
// RTMClient.Register
// IApplicationRegister.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTMClient.Module;

namespace RTMClient.Register
{
    public interface IApplicationRegister
    {
        void AddModule<T>() where T : IModule;
    }
}