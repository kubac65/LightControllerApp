// <copyright file="DeviceDiscoveredEventArgs.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;

    public class DeviceDiscoveredEventArgs : EventArgs
    {
        public IPAddress Address { get; set; }

        public byte[] Mac { get; set; }
    }
}