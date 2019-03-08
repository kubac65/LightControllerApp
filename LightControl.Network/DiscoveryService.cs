// <copyright file="DiscoveryService.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// <see cref="DiscoveryService "/> discovers control devices connected to the same network.
    /// </summary>
    internal class DiscoveryService : IDisposable
    {
        private readonly int _port;
        private readonly Socket _socket;
        private readonly IPEndPoint _src;
        private bool _listen = false;
        private Thread _worker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryService"/> class.
        /// </summary>
        /// <param name="port">Port used to listen for control devices</param>
        public DiscoveryService(int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _port = port;
            _src = new IPEndPoint(IPAddress.Any, port);
            _socket.Bind(_src);
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
                        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.None, 0);
                        int bytes = _socket.Available;
                        if (bytes >= 8)
                        {
                            // Read preamble and verify that it's correct
                            byte[] message = new byte[8];
                            _socket.ReceiveFrom(message, SocketFlags.None, ref remoteEndPoint);
                            if (message[0] != 0xff || message[1] != 0xff)
                            {
                                // Preamble incorrect
                                continue;
                            }

                            byte[] macBytes = message.Skip(2).ToArray();
                            PhysicalAddress mac = new PhysicalAddress(macBytes);
                            var eventArgs = new DeviceDiscoveredEventArgs(((IPEndPoint)remoteEndPoint).Address, mac);
                            DeviceDiscovered?.Invoke(this, eventArgs);
                        }

                        Thread.Sleep(1);
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
            _socket.Dispose();
        }
    }
}