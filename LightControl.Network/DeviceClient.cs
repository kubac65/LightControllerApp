// <copyright file="DeviceClient.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Client used for comunicating with control device.
    /// </summary>
    public class DeviceClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="address">IP address of the control device.</param>
        /// <param name="port">Listening port of the control device.</param>
        public DeviceClient(IPAddress address, int port)
        {
            _client = new TcpClient();
            _client.Connect(new IPEndPoint(address, port));
            _stream = _client.GetStream();
        }

        public string SendEcho(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            _stream.Write(data, 0, data.Length);

            byte[] response = new byte[data.Length];
            int bytesRead = _stream.Read(response, 0, data.Length);
            string receivedMsg = Encoding.ASCII.GetString(response);
            return receivedMsg;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _stream.Close();
            _client.Close();
        }
    }
}