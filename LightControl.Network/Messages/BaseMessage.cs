// <copyright file="BaseMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    /// <summary>
    /// <see cref="BaseMessage"/> provides a low level wrapper for the network message.
    /// </summary>
    internal abstract class BaseMessage
    {
        private const int HeaderLength = 4;

        private readonly Command _command;
        private readonly byte[] _payload;
        private readonly Flag _flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessage"/> class.
        /// </summary>
        /// <param name="command">Command to be executed on the device.</param>
        /// <param name="flags">Message flags.</param>
        public BaseMessage(Command command, params Flag[] flags)
        {
            _command = command;
            _payload = new byte[0];
            foreach (var f in flags)
            {
                _flags &= f;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMessage"/> class.
        /// </summary>
        /// <param name="command">Command to be executed on the device.</param>
        /// <param name="payload">Payload to be sent with the message.</param>
        /// <param name="flags">Message flags.</param>
        public BaseMessage(Command command, byte[] payload, params Flag[] flags)
            : this(command, flags)
        {
            _payload = payload;
        }

        /// <summary>
        /// Serializes current message to a byte array.
        /// </summary>
        /// <returns>Serialized message</returns>
        public byte[] Serialize()
        {
            // Header of the packet consists of 3 fields: command, flags and payload length
            // |---8----|---8----|--------16------| -> size
            // |--COM---|--FLG---|-------LEN------| -> legend
            int packetSize = HeaderLength + _payload.Length;
            byte[] message = new byte[packetSize];

            message[0] = (byte)_command;
            message[1] = (byte)_flags;

            // Get payload length
            message[2] = (byte)_payload.Length;
            message[3] = (byte)(_payload.Length >> 8);

            // Append payload
            return message;
        }
    }
}