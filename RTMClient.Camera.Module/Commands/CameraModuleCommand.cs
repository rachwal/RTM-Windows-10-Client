// RTMClient
// RTMClient.Camera.Module
// CameraModuleCommand.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using System;
using System.Windows.Input;

namespace RTMClient.Camera.Module.Commands
{
    public class CameraModuleCommand : ICommand
    {
        private readonly Action action;

        public CameraModuleCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}