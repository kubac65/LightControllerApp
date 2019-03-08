// <copyright file="Device.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.DeviceManagement
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.NetworkInformation;

    /// <summary>
    /// <see cref="Device"/> class provides abstraction for communicating with the end device.
    /// </summary>
    public class Device
    {
        private DeviceClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="ipAddress">IP address of the device.</param>
        /// <param name="port">Port used for communication with the device.</param>
        /// <param name="mac">MAC address of the device.</param>
        public Device(IPAddress ipAddress, int port, PhysicalAddress mac)
        {
            IPAddress = ipAddress;
            Mac = mac;
            _client = new DeviceClient(IPAddress, port);
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
        /// Gets or sets device's last seen time.
        /// </summary>
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Gets available outputs.
        /// </summary>
        public IEnumerable<DeviceOutput> Outputs
        {
            get
            {
                if (_client.Connected)
                {
                    var res = _client.GetAvailableOutputs();

                    for (int i = 0; i < res.Ids.Length; i++)
                    {
                        yield return new DeviceOutput(res.Ids[i], res.States[i], _client);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether device connection is open.
        /// </summary>
        public bool Connected
        {
            get
            {
                return _client.Connected;
            }
        }

        /// <summary>
        /// Connects to a device.
        /// </summary>
        public void Connect()
        {
            if (!_client.Connected)
            {
                _client.Connect();
            }
        }

        /// <summary>
        /// Disconnects from a device.
        /// </summary>
        public void Disconnect()
        {
            if (_client.Connected)
            {
                _client.Disconnect();
            }
        }
    }
}