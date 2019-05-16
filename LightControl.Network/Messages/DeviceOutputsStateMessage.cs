// <copyright file="DeviceOutputsStateMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <see cref="DeviceOutputsStateMessage"/> represents a message contains Ids of available outputs and their states.
    /// </summary>
    internal class DeviceOutputsStateMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOutputsStateMessage"/> class.
        /// </summary>
        /// <param name="flags">Message flags</param>
        private DeviceOutputsStateMessage(Flag flags)
            : base(Command.GetOutputs, flags)
        {
        }

        /// <summary>
        /// Gets Ids of available outputs.
        /// </summary>
        public int[] Ids { get; private set; }

        /// <summary>
        /// Gets States of available outputs.
        /// </summary>
        public bool[] States { get; private set; }

        /// <summary>
        /// Deserializes bytes into message object
        /// </summary>
        /// <param name="bytes">Received bytes</param>
        /// <returns>An instance of deserilized <see cref="DeviceOutputsRequestMessage"/> object</returns>
        public static DeviceOutputsStateMessage Deserialize(byte[] bytes)
        {
            Command command = GetCommand(bytes);
            if (command != Command.GetOutputs)
            {
                throw new Exception(); // TODO add own exception hierarchy
            }

            int payloadLength = GetPayloadLength(bytes);
            byte[] payload = bytes.Skip(HeaderLength).Take(payloadLength).ToArray();
            List<int> ids = new List<int>();
            List<bool> states = new List<bool>();
            foreach (byte b in payload)
            {
                // MSB is used to indicate the state of the output
                // while remaining 7 bits represent the Id of the output
                int id = b & 0x7f;
                bool state = (b >> 7) == 1;

                ids.Add(id);
                states.Add(state);
            }

            Flag flags = GetFlags(bytes);
            var msg = new DeviceOutputsStateMessage(flags)
            {
                Ids = ids.ToArray(),
                States = states.ToArray(),
                Payload = payload
            };
            return msg;
        }
    }
}