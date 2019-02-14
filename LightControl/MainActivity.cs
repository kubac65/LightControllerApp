// <copyright file="MainActivity.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl
{
    using Android.App;
    using Android.OS;
    using Android.Support.V7.App;
    using Android.Widget;
    using LightControl.Adapters;
    using LightControl.Models;
    using LightControl.Network;
    using System.Collections.Generic;
    using System.Text;

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private readonly List<DeviceModel> _devices = new List<DeviceModel>();
        private DiscoveryService _ds = new DiscoveryService(DefaultConfiguration.BroadcastPort);

        private List<string> _ips = new List<string>();

        private DeviceListAdapter _adapter;
        private ListView _deviceListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _ds.DeviceDiscovered += Ds_DeviceDiscovered;
            _ds.BeginReceive();

            _deviceListView = FindViewById<ListView>(Resource.Id.deviceList);

            _adapter = new DeviceListAdapter(this, Resource.Id.deviceList, _devices);
            _deviceListView.Adapter = _adapter;
        }

        private void Ds_DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs)
        {
            string receiveString = Encoding.ASCII.GetString(eventArgs.Mac);
            if (_ips.Contains(eventArgs.Address.ToString()))
            {
                return;
            }

            _ips.Add(eventArgs.Address.ToString());

            RunOnUiThread(() =>
            {
                _adapter.Add(new DeviceModel
                {
                    Name = eventArgs.Address.ToString(),
                    IPAddress = eventArgs.Address.ToString(),
                    Mac = receiveString
                });
                _adapter.NotifyDataSetChanged();
            });
        }
    }
}