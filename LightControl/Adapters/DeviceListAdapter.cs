// <copyright file="DeviceListAdapter.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Adapters
{
    using Android.Content;
    using Android.Views;
    using Android.Widget;
    using LightControl.Network.DeviceManagement;
    using System.Linq;

    /// <summary>
    /// Adapts <see cref="Device"/> object to display them in the <see cref="ListView"/>.
    /// </summary>
    internal class DeviceListAdapter : ArrayAdapter<Device>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceListAdapter"/> class.
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="textViewResourceId">Resource Id.</param>
        public DeviceListAdapter(Context context, int textViewResourceId)
            : base(context, textViewResourceId)
        {
        }

        /// <inheritdoc/>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var device = GetItem(position);
            var inflater = LayoutInflater.From(Context);
            convertView = convertView ?? inflater.Inflate(Resource.Layout.device_list_item, parent, false);
            var deviceName = convertView.FindViewById<TextView>(Resource.Id.device_name);
            var deviceIp = convertView.FindViewById<TextView>(Resource.Id.device_ip_address);
            var deviceMac = convertView.FindViewById<TextView>(Resource.Id.device_mac_address);
            var staus = convertView.FindViewById<TextView>(Resource.Id.device_status);

            deviceName.Text = device.Name;
            deviceIp.Text = device.IPAddress.ToString();
            deviceMac.Text = string.Join(":", device.Mac.GetAddressBytes().Select(b => b.ToString("X2")));
            staus.Text = device.Available ? "Available" : "Not Available";
            convertView.Click += (e, d) => { };

            return convertView;
        }
    }
}