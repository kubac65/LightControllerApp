// <copyright file="OutputsRequestMessage.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.Messages
{
    internal class OutputsRequestMessage : BaseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputsRequestMessage"/> class.
        /// </summary>
        public OutputsRequestMessage()
            : base(Command.GetOutputs, Flag.Request)
        {
        }
    }
}