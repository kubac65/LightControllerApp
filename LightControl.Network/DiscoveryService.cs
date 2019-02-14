// <copyright file="DiscoveryService.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public delegate void DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs);

    public class DiscoveryService : IDisposable
    {
        private readonly int _port;
        private readonly UdpClient _udp = new UdpClient();
        private IPEndPoint _src;

        public event DeviceDiscovered DeviceDiscovered;

        public DiscoveryService(int port)
        {
            this._port = port;
            _src = new IPEndPoint(IPAddress.Any, port);

            _udp.Client.Bind(_src);
        }

        public void BeginReceive()
        {
            _udp.BeginReceive(new AsyncCallback(ReceiveMessage), this);
        }

        public void Dispose()
        {
            _udp.Dispose();
        }

        private void ReceiveMessage(IAsyncResult ar)
        {
            byte[] receivedMessage = _udp.EndReceive(ar, ref _src);
            DeviceDiscovered?.Invoke(this, new DeviceDiscoveredEventArgs()
            {
                Address = _src.Address,
                Mac = receivedMessage
            });

            BeginReceive();
        }
    }
}