// <copyright file="DeviceDiscoveredEventArgs.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;

    /// <summary>
    /// Event arguments emited by <see cref="DiscoveryService.DeviceDiscovered"/> event.
    /// </summary>
    public class DeviceDiscoveredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDiscoveredEventArgs"/> class.
        /// </summary>
        /// <param name="address">IP address of discovered control device</param>
        /// <param name="mac">MAC address if discovered control device</param>
        public DeviceDiscoveredEventArgs(IPAddress address, byte[] mac)
        {
            Address = address;
            Mac = mac;
        }

        /// <summary>
        /// Gets IP address of discovered control device.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        /// Gets MAC address if discovered control device
        /// </summary>
        public byte[] Mac { get; }
    }
}