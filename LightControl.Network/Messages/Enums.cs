// <copyright file="Enums.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    using System;

    /// <summary>
    /// Indicates command to be executed on a device.
    /// </summary>
    internal enum Command
    {
        /// <summary>
        /// Command sent to a device to get its available outputs
        /// </summary>
        GetOutputs = 0x1,

        /// <summary>
        /// Command sent to a device to switch out of its outputs
        /// </summary>
        SwitchOutput = 0x2,
    }

    /// <summary>
    /// Message flags
    /// </summary>
    [Flags]
    internal enum Flag
    {
        /// <summary>
        /// Flag indicating that a message is a requests
        /// </summary>
        Request = 0x1,

        /// <summary>
        /// Flag indicating that a message is a response
        /// </summary>
        Response = 0x2
    }
}