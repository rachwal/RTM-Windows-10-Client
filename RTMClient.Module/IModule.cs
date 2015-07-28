// RTMClient
// RTMClient.Module
// IModule.cs
// 
// Created by Bartosz Rachwal.
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved.

using System;
using System.Collections.Generic;

namespace RTMClient.Module
{
    public interface IModule
    {
        void Initialize();
        Dictionary<string, Type> ViewTypes { get; }
        Dictionary<Type, Type> ViewModelTypes { get; }
    }
}