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
        private readonly int port;
        private readonly UdpClient udp = new UdpClient();
        private IPEndPoint src;

        public event DeviceDiscovered DeviceDiscovered;

        public DiscoveryService(int port)
        {
            this.port = port;
            src = new IPEndPoint(IPAddress.Any, port);

            udp.Client.Bind(src);
        }

        public void BeginReceive()
        {
            udp.BeginReceive(new AsyncCallback(this.ReceiveMessage), this);
        }

        public void Dispose()
        {
            udp.Dispose();
        }

        private void ReceiveMessage(IAsyncResult ar)
        {
            byte[] receivedMessage = udp.EndReceive(ar, ref src);
            DeviceDiscovered?.Invoke(this, new DeviceDiscoveredEventArgs()
            {
                Address = src.Address,
                Mac = receivedMessage
            });

            BeginReceive();
        }
    }
}