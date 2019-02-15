// <copyright file="DeviceListAdapter.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Adapters
{
    using System.Collections.Generic;
    using System.Net;
    using Android.Content;
    using Android.Views;
    using Android.Widget;
    using LightControl.Models;
    using LightControl.Network;

    internal class DeviceListAdapter : ArrayAdapter<DeviceModel>, View.IOnClickListener
    {
        public DeviceListAdapter(Context context, int textViewResourceId, IList<DeviceModel> objects)
            : base(context, textViewResourceId, objects)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var o = GetItem(position);
            var inflater = LayoutInflater.From(Context);
            convertView = inflater.Inflate(Resource.Layout.device_list_item, parent, false);
            var deviceName = convertView.FindViewById<TextView>(Resource.Id.device_name);
            var deviceIp = convertView.FindViewById<TextView>(Resource.Id.device_ip_address);
            var deviceMac = convertView.FindViewById<TextView>(Resource.Id.device_mac_address);

            deviceName.Text = o.Name;
            deviceIp.Text = o.IPAddress;
            deviceMac.Text = o.Mac;

            convertView.SetOnClickListener(this);

            return convertView;
        }

        /// <inheritdoc/>
        void View.IOnClickListener.OnClick(View v)
        {
            var ip = v.FindViewById<TextView>(Resource.Id.device_ip_address);

            var ipAddr = IPAddress.Parse(ip.Text);
            var d = new DeviceClient(ipAddr, DefaultConfiguration.ConnectionPort);
            d.SendEcho("test");
        }
    }
}