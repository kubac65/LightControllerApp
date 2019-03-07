// <copyright file="Message.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    /// <summary>
    /// <see cref="Message"/> provides a low level wrapper for the network message.
    /// </summary>
    internal abstract class Message
    {
        /// <summary>
        /// Header length
        /// </summary>
        public const int HeaderLength = 4;

        /// <summary>
        /// Maximum payload length
        /// </summary>
        public const int MaxPayloadLength = 65535; // 2 byte integer

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="command">Command to be executed on the device.</param>
        /// <param name="flags">Flags set on the message.</param>
        public Message(Command command, Flag flags)
        {
            Command = command;
            Flags = flags;
        }

        /// <summary>
        /// Gets or sets serialized payload.
        /// </summary>
        public byte[] Payload { get; protected set; } = new byte[0];

        /// <summary>
        /// Gets command.
        /// </summary>
        public Command Command { get; private set; }

        /// <summary>
        /// Gets flags set on this message
        /// </summary>
        public Flag Flags { get; private set; }

        /// <summary>
        /// Serializes current message to a byte array.
        /// </summary>
        /// <returns>Serialized message</returns>
        public byte[] Serialize()
        {
            // Header of the packet consists of 3 fields: command, flags and payload length
            // |---8----|---8----|--------16------| -> size
            // |--COM---|--FLG---|-------LEN------| -> legend
            int packetSize = HeaderLength + Payload.Length;
            byte[] bytes = new byte[packetSize];

            bytes[0] = (byte)Command;
            bytes[1] = (byte)Flags;

            // Get payload length
            bytes[2] = (byte)Payload.Length;
            bytes[3] = (byte)(Payload.Length >> 8);

            // Append payload
            for (int i = HeaderLength; i < packetSize; i++)
            {
                bytes[i] = Payload[i - HeaderLength];
            }

            return bytes;
        }

        /// <summary>
        /// Gets command from serialised message
        /// </summary>
        /// <param name="bytes">Received message</param>
        /// <returns>Command</returns>
        protected static Command GetCommand(byte[] bytes)
        {
            return (Command)bytes[0];
        }

        /// <summary>
        /// Extracts flags from received message
        /// </summary>
        /// <param name="bytes">Received message</param>
        /// <returns>Array of flags</returns>
        protected static Flag GetFlags(byte[] bytes)
        {
            return (Flag)bytes[1];
        }

        /// <summary>
        /// Gets payload to that will be included in the message
        /// </summary>
        /// <returns>Serialized payload</returns>
        protected virtual byte[] GetPayload()
        {
            return new byte[0];
        }
    }
}