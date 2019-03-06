// <copyright file="ToggleOutputRequestMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    internal class ToggleOutputRequestMessage : BaseMessage
    {
        public ToggleOutputRequestMessage(Command command, params Flag[] flags)
            : base(command, flags)
        {
        }
    }
}