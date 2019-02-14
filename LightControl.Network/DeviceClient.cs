// <copyright file="DeviceClient.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    internal class DeviceClient : IDisposable
    {
        private TcpClient _client;

        public DeviceClient(IPAddress address, int port)
        {
            _client = new TcpClient();
            _client.Connect(new IPEndPoint(address, port));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}