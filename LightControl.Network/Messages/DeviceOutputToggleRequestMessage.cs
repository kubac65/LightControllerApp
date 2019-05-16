// <copyright file="DeviceOutputToggleRequestMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    /// <summary>
    /// <see cref="DeviceOutputToggleRequestMessage"/> represents a message sent to the device to toggle one of its outputs
    /// </summary>
    internal class DeviceOutputToggleRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOutputToggleRequestMessage"/> class.
        /// </summary>
        /// <param name="id">Id of the output to be toggled.</param>
        /// <param name="state">Desired state of the output.</param>
        public DeviceOutputToggleRequestMessage(int id, bool state)
            : base(Command.SwitchOutput, Flag.Request)
        {
            Id = id;
            State = state;

            Payload = new byte[1]
            {
                (byte)Id
            };

            if (state)
            {
                // Toggle state bit
                Payload[0] |= 0x80;
            }
        }

        /// <summary>
        /// Gets id of the output
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets a value indicating whether output will be toggled on or off.
        /// </summary>
        public bool State { get; }
    }
}