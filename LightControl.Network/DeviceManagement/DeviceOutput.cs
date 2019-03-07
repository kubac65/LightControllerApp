// <copyright file="DeviceOutput.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.DeviceManagement
{
    using System;

    /// <summary>
    /// <see cref="DeviceOutput"/> provides abstraction for interacting with device output.
    /// </summary>
    public class DeviceOutput
    {
        private readonly DeviceClient _client;
        private bool _isOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceOutput"/> class.
        /// </summary>
        /// <param name="id">Id of the output</param>
        /// <param name="isOn">Output state</param>
        /// <param name="client">Device client of parent device</param>
        internal DeviceOutput(int id, bool isOn, DeviceClient client)
        {
            Id = id;
            _isOn = isOn;
            _client = client;
        }

        /// <summary>
        /// Gets or sets output name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets output Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether output is on.
        /// </summary>
        public bool IsOn
        {
            get
            {
                return _isOn;
            }

            set
            {
                var res = _client.ToggleOutput(Id, value);

                var idx = Array.IndexOf(res.Ids, Id);
                if (idx == -1)
                {
                    throw new Exception(); // Somehow output does not exist anymore
                }

                // Check that output was infact toggled, for now ignore the fact that it may end up in the same state as before
                bool resultedState = res.States[idx] == value;
                _isOn = resultedState;
            }
        }
    }
}