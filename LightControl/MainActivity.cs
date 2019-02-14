// <copyright file="MainActivity.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl
{
    using System.Collections.Generic;
    using System.Text;
    using Android.App;
    using Android.OS;
    using Android.Support.V7.App;
    using Android.Widget;
    using LightControl.Adapters;
    using LightControl.Models;
    using LightControl.Network;

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private DiscoveryService ds = new DiscoveryService(DefaultConfiguration.BroadcastPort);

        private List<string> ips = new List<string>();

        private DeviceListAdapter adapter;
        private List<DeviceModel> devices = new List<DeviceModel>();
        private ListView deviceListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ds.DeviceDiscovered += Ds_DeviceDiscovered;
            ds.BeginReceive();

            deviceListView = FindViewById<ListView>(Resource.Id.deviceList);

            adapter = new DeviceListAdapter(this, Resource.Id.deviceList, devices);
            deviceListView.Adapter = adapter;
        }

        private void Ds_DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs)
        {
            string receiveString = Encoding.ASCII.GetString(eventArgs.Mac);
            if (ips.Contains(eventArgs.Address.ToString()))
            {
                return;
            }

            ips.Add(eventArgs.Address.ToString());

            this.RunOnUiThread(() =>
            {
                adapter.Add(new DeviceModel
                {
                    Name = eventArgs.Address.ToString(),
                    IPAddress = eventArgs.Address.ToString(),
                    Mac = receiveString
                });
                adapter.NotifyDataSetChanged();
            });

            //var label = new TextView(this);
            //label.Text = eventArgs.Address.ToString();
            //devices.AddView(label);
        }
    }
}