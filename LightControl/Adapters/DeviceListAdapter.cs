// <copyright file="DeviceListAdapter.cs" company="Jakub Potocki">
// Copyright (c) Jakub Potocki. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace LightControl.Adapters
{
    using Android.App;
    using Android.Views;
    using Android.Widget;
    using Java.Lang;
    using LightControl.Network.DeviceManagement;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Adapts <see cref="Device"/> object to display them in the <see cref="ListView"/>.
    /// </summary>
    internal class DeviceListAdapter : BaseExpandableListAdapter
    {
        private readonly Activity _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceListAdapter"/> class.
        /// </summary>
        /// <param name="context">Calling activity</param>
        public DeviceListAdapter(Activity context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets list of devices
        /// </summary>
        public List<Device> Devices { get; } = new List<Device>();

        /// <inheritdoc/>
        public override int GroupCount => Devices.Count;

        /// <inheritdoc/>
        public override bool HasStableIds => true;

        /// <inheritdoc/>
        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        /// <inheritdoc/>
        public override int GetChildrenCount(int groupPosition)
        {
            var device = Devices[groupPosition];

            if (device.Available)
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc/>
        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var inflater = _context.LayoutInflater;
            convertView = convertView ?? inflater.Inflate(Resource.Layout.device_controls, null);

            var device = Devices[groupPosition];
            foreach (var o in device.Outputs)
            {

            }

            return convertView;
        }

        /// <inheritdoc/>
        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        /// <inheritdoc/>
        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var inflater = _context.LayoutInflater;
            convertView = convertView ?? inflater.Inflate(Resource.Layout.device_list_item, null);
            var deviceName = convertView.FindViewById<TextView>(Resource.Id.device_name);
            var deviceIp = convertView.FindViewById<TextView>(Resource.Id.device_ip_address);
            var deviceMac = convertView.FindViewById<TextView>(Resource.Id.device_mac_address);
            var status = convertView.FindViewById<TextView>(Resource.Id.device_status);
            deviceName.Left = 40;
            deviceIp.Left = 40;
            deviceMac.Left = 40;

            var device = Devices[groupPosition];
            deviceName.Text = device.Name;
            deviceIp.Text = device.IPAddress.ToString();
            deviceMac.Text = string.Join(":", device.Mac.GetAddressBytes().Select(b => b.ToString("X2")));
            status.Text = device.Available ? "Available" : "Not Available";

            // Disconnect from the device when group gets collapsed
            if (isExpanded)
            {
                device.Connect();
            }
            else if (!isExpanded)
            {
                device.Disconnect();
            }

            return convertView;
        }

        /// <inheritdoc/>
        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        /// Not needed
        /// <inheritdoc/>
        public override Object GetChild(int groupPosition, int childPosition)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public override Object GetGroup(int groupPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}