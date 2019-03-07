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
    using LightControl.Network.DeviceManagement;

    /// <summary>
    /// Main activity displaying the lsit of discovered devices
    /// </summary>
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private DeviceManager _ds;
        private DeviceListAdapter _adapter;

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _ds = new DeviceManager();
            _ds.DeviceDiscovered += Ds_DeviceDiscovered;
            _ds.DeviceAvailable += Ds_DeviceAvailable;
            _ds.DeviceNotAvailable += Ds_DeviceNotAvailable;

            var deviceListView = FindViewById<ExpandableListView>(Resource.Id.device_list);
            _adapter = new DeviceListAdapter(this);
            deviceListView.SetAdapter(_adapter);
        }

        private void Ds_DeviceDiscovered(object sender, Device device)
        {
            RunOnUiThread(() =>
            {
                _adapter.Devices.Add(device);
                _adapter.NotifyDataSetChanged();
                Toast.MakeText(this, "Device discovered", ToastLength.Long).Show();
            });
        }

        private void Ds_DeviceNotAvailable(object sender, Device device)
        {
            RunOnUiThread(() =>
            {
                _adapter.NotifyDataSetChanged();
                Toast.MakeText(this, "Device not available", ToastLength.Long).Show();
            });
        }

        private void Ds_DeviceAvailable(object sender, Device device)
        {
            RunOnUiThread(() =>
            {
                _adapter.NotifyDataSetChanged();
                Toast.MakeText(this, "Device available", ToastLength.Long).Show();
            });
        }
    }
}