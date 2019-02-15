// <copyright file="DiscoveryService.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Delegate definition used for <see cref="DiscoveryService.DeviceDiscovered"/> event.
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="eventArgs">Event arguments</param>
    public delegate void DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs);

    /// <summary>
    /// <see cref="DiscoveryService "/> discovers control devices connected to the same network.
    /// </summary>
    public class DiscoveryService : IDisposable
    {
        private readonly int _port;
        private readonly UdpClient _udp = new UdpClient();
        private IPEndPoint _src;

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
        public event DeviceDiscovered DeviceDiscovered;

        /// <summary>
        /// Starts listening to incoming messages.
        /// </summary>
        public void BeginReceive()
        {
            _udp.BeginReceive(new AsyncCallback(ReceiveMessage), this);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _udp.Dispose();
        }

        private void ReceiveMessage(IAsyncResult ar)
        {
            byte[] receivedMessage = _udp.EndReceive(ar, ref _src);
            var eventArgs = new DeviceDiscoveredEventArgs(_src.Address, receivedMessage);
            DeviceDiscovered?.Invoke(this, eventArgs);
            BeginReceive();
        }
    }
}