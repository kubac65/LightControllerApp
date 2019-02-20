// <copyright file="DiscoveryService.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// <see cref="DiscoveryService "/> discovers control devices connected to the same network.
    /// </summary>
    internal class DiscoveryService : IDisposable
    {
        private readonly int _port;
        private readonly UdpClient _udp = new UdpClient();
        private IPEndPoint _src;
        private bool _listen = false;
        private Thread _worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryService"/> class.
        /// </summary>
        /// <param name="port">Port used to listen for control devices</param>
        public DiscoveryService(int port)
        {
            _port = port;
            _src = new IPEndPoint(IPAddress.Any, port);
            _udp.Client.Bind(_src);
        }

        /// <summary>
        /// <see cref="DeviceDiscovered"/> event raised every time the device discovery message is received.
        /// </summary>
        public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscovered;

        /// <summary>
        /// Starts listening for device broadcasts.
        /// </summary>
        public void Start()
        {
            if (!_listen)
            {
                _worker = new Thread(() =>
                {
                    while (_listen)
                    {
                        byte[] receivedMessage = _udp.Receive(ref _src);
                        var eventArgs = new DeviceDiscoveredEventArgs(_src.Address, receivedMessage);
                        DeviceDiscovered?.Invoke(this, eventArgs);
                    }
                });
                _worker.Start();
                _listen = true;
            }
            else
            {
                throw new InvalidOperationException("Discovery service is already started");
            }
        }

        /// <summary>
        /// Stops listening to device broadcasts.
        /// </summary>
        public void Stop()
        {
            _listen = false;
            _worker.Abort(); // TODO: This may not be necessary
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _udp.Dispose();
        }
    }
}