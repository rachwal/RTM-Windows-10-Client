// RTMClient
// RTMClient.Module
// IViewModel.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using Windows.UI.Xaml.Navigation;

namespace RTMClient.Module
{
    public interface IViewModel
    {
        void StartCamera(NavigationEventArgs args);
        void StopCamera(NavigationEventArgs args);
    }
}