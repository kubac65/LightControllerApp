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
        private TcpClient client;

        public DeviceClient(IPAddress address, int port)
        {
            client = new TcpClient();
            client.Connect(new IPEndPoint(address, port));
        }

        //public string Echo(string value)
        //{
        //    using (var stream = new MemoryStream(client.GetStream()))
        //    {

        //    }
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}