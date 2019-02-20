// <copyright file="Device.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.DeviceManagement
{
    using System;
    using System.Net;

    public class Device
    {
        private readonly int _port;
        private volatile bool _available;
        private DateTime _lastSeen;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="mac"></param>
        public Device(IPAddress ipAddress, int port, byte[] mac)
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
        public byte[] Mac { get; private set; }

        /// <summary>
        /// Gets or sets name of this device.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether this device is available.
        /// </summary>
        public bool Available { get => _available; internal set => _available = value; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public DateTime LastSeen
        {
            get
            {
                lock (this)
                {
                    return _lastSeen;
                }
            }

            set
            {
                lock (this)
                {
                    _lastSeen = value;
                }
            }
        }
    }
}