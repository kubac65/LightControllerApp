// <copyright file="Device.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.DeviceManagement
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;

    /// <summary>
    /// <see cref="Device"/> class provides abstraction for communicating with the end device.
    /// </summary>
    public class Device
    {
        private readonly int _port;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="ipAddress">IP address of the device.</param>
        /// <param name="port">Port used for communication with the device.</param>
        /// <param name="mac">MAC address of the device.</param>
        public Device(IPAddress ipAddress, int port, PhysicalAddress mac)
        {
            IPAddress = ipAddress;
            _port = port;
            Mac = mac;
        }

        /// <summary>
        /// Gets IP address of this device.
        /// </summary>
        public IPAddress IPAddress { get; private set; }

        /// <summary>
        /// Gets MAC address of this device.
        /// </summary>
        public PhysicalAddress Mac { get; private set; }

        /// <summary>
        /// Gets or sets name of this device.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this device is available.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public DateTime LastSeen { get; set; }
    }
}