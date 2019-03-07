// <copyright file="DeviceOutputsRequestMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    /// <summary>
    /// <see cref="DeviceOutputsRequestMessage"/> represents a message sent to device to request states of available outputs
    /// </summary>
    internal class DeviceOutputsRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOutputsRequestMessage"/> class.
        /// </summary>
        public DeviceOutputsRequestMessage()
            : base(Command.GetOutputs, Flag.Request)
        {
        }
    }
}