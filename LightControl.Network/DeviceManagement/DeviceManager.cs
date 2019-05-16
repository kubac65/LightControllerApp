// <copyright file="DeviceManager.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Network.DeviceManagement
{
    using LightControl.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;
    using System.Timers;

    /// <summary>
    /// <see cref="DeviceManager"/> is responsible for discovering and maintaining the registry of available devices.
    /// </summary>
    public class DeviceManager : IDisposable
    {
        private readonly DiscoveryService _discoveryService = new DiscoveryService(DefaultConfiguration.BroadcastPort);
        private readonly Dictionary<IPAddress, Device> _deviceLookup = new Dictionary<IPAddress, Device>();
        private readonly Timer _deviceWatchdog = new Timer(DefaultConfiguration.WatchdogPeriod);

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceManager"/> class.
        /// </summary>
        public DeviceManager()
        {
            _deviceWatchdog.Elapsed += DeviceWatchdog_Elapsed;
            _deviceWatchdog.Enabled = true;

            _discoveryService.DeviceDiscovered += DiscoveryService_DeviceDiscovered;
            _discoveryService.Start();
        }

        /// <summary>
        /// Event raised when new device is discovered.
        /// </summary>
        public event EventHandler<Device> DeviceDiscovered;

        /// <summary>
        /// Event raised when existing device is available again.
        /// </summary>
        public event EventHandler<Device> DeviceAvailable;

        /// <summary>
        /// Event raised when existing device becomes not available.
        /// </summary>
        public event EventHandler<Device> DeviceNotAvailable;

        /// <inheritdoc/>
        public void Dispose()
        {
            _discoveryService.Stop();
            _deviceWatchdog.Stop();
        }

        private void DeviceWatchdog_Elapsed(object sender, ElapsedEventArgs e)
        {
            var currentTimestamp = DateTime.UtcNow;
            Task.Factory.StartNew(() =>
            {
                lock (_deviceLookup)
                {
                    foreach (var d in _deviceLookup.Values.Where(d => d.Available))
                    {
                        var diff = currentTimestamp - d.LastSeen;
                        if (diff > TimeSpan.FromMilliseconds(DefaultConfiguration.DeviceTimeout) && !d.Connected)
                        {
                            d.Available = false;
                            DeviceNotAvailable?.Invoke(this, d);
                        }
                    }
                }
            });
        }

        private void DiscoveryService_DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs)
        {
            // When discovery service receives broadcast check that we already have the device created.
            // If device exists, update it's LastSeen property. Otherwise add new device to the lookup
            var currentTimestamp = DateTime.UtcNow;
            Task.Factory.StartNew(() =>
            {
                lock (_deviceLookup)
                {
                    if (!_deviceLookup.TryGetValue(eventArgs.Address, out Device d))
                    {
                        d = new Device(eventArgs.Address, DefaultConfiguration.ConnectionPort, eventArgs.Mac)
                        {
                            Available = true,
                        };

                        _deviceLookup.Add(eventArgs.Address, d);
                        DeviceDiscovered?.Invoke(this, d); // NOTE: Event handlers are executed inside lock!!!
                    }

                    d.LastSeen = currentTimestamp;
                    if (!d.Available)
                    {
                        d.Available = true;
                        DeviceAvailable?.Invoke(this, d); // NOTE: Event handlers are executed inside lock!!!
                    }
                }
            });
        }
    }
}