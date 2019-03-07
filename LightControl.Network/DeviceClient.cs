// <copyright file="DeviceClient.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    using LightControl.Network.Messages;
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Client used for comunicating with control device.
    /// </summary>
    internal class DeviceClient : IDisposable
    {
        private readonly IPAddress _address;
        private readonly int _port;
        private readonly TcpClient _client;

        private NetworkStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceClient"/> class.
        /// </summary>
        /// <param name="address">IP address of the control device.</param>
        /// <param name="port">Listening port of the control device.</param>
        public DeviceClient(IPAddress address, int port)
        {
            _address = address;
            _port = port;
            _client = new TcpClient();
        }

        public bool Connected => _client.Connected;

        public void Connect()
        {
            _client.Connect(new IPEndPoint(_address, _port));
            _stream = _client.GetStream();
        }

        public void Disconnect()
        {
        }

        public DeviceOutputsStateMessage GetAvailableOutputs()
        {
            byte[] bytes = SendMessage(new DeviceOutputsRequestMessage());
            return DeviceOutputsStateMessage.Deserialize(bytes);
        }

        public DeviceOutputsStateMessage ToggleOutput(int id, bool state)
        {
            byte[] bytes = SendMessage(new DeviceOutputToggleRequestMessage(id, state));
            return DeviceOutputsStateMessage.Deserialize(bytes);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _stream.Close();
            _client.Close();
        }

        private byte[] SendMessage(Message request)
        {
            var serialisedMessage = request.Serialize();
            _stream.Write(serialisedMessage, 0, serialisedMessage.Length);
            _stream.Flush();

            int bufferSize = Message.HeaderLength + Message.MaxPayloadLength;
            byte[] responseBuffer = new byte[bufferSize];
            _stream.Read(responseBuffer, 0, bufferSize);
            return responseBuffer;
        }
    }
}