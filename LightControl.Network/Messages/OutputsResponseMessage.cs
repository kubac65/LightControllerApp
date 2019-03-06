// <copyright file="OutputsResponseMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    internal class OutputsResponseMessage : BaseMessage
    {
        public OutputsResponseMessage(Command command, byte[] payload, params Flag[] flags)
            : base(command, payload, flags)
        {
        }
    }
}