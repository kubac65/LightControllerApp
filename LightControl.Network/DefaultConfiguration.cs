﻿// <copyright file="DefaultConfiguration.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network
{
    /// <summary>
    /// Default network configuration.
    /// </summary>
    public static class DefaultConfiguration
    {
        /// <summary>
        /// Gets port used for broadcasting messages.
        /// </summary>
        public static int BroadcastPort => 54545;

        /// <summary>
        /// Gets port used by control devices to listen for incoming connections.
        /// </summary>
        public static int ConnectionPort => 54546;

        /// <summary>
        /// Gets device wachdog period used for monitoring devices.
        /// </summary>
        public static int WatchdogPeriod => 1000;

        /// <summary>
        /// Gets device timeout time.
        /// </summary>
        public static int DeviceTimeout => 4000; // Since we're using UDP we have to allow for lost packets
    }
}