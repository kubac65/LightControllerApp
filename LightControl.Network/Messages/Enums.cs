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
        GetOutputs = 0x1,
        SwitchOutput = 0x2,
    }

    [Flags]
    internal enum Flag
    {
        Request = 0x1,
        Response = 0x2
    }
}