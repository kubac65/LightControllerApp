using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using LightControl.Network;
using System.Text;
using System.Collections.Generic;
using Android.Content;

namespace LightControl
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private DiscoveryService ds = new DiscoveryService();
        private List<string> ips = new List<string>();

        private ArrayAdapter<string> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            items = new ArrayAdapter<string>(this, Resource.Id.devices, ips);
                       
            ds.DeviceDiscovered += Ds_DeviceDiscovered;
            ds.BeginReceive();
        }

        private void Ds_DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs)
        {
            var devices = this.FindViewById<ListView>(Resource.Id.devices);
            string receiveString = Encoding.ASCII.GetString(eventArgs.Mac);
            if (ips.Contains(eventArgs.Address.ToString()))
            {
                return;
            }

            ips.Add(eventArgs.Address.ToString());
            this.RunOnUiThread(() =>
            {
                items.Add(eventArgs.Address.ToString());
                devices.Adapter = items;
            });

            //var label = new TextView(this);
            //label.Text = eventArgs.Address.ToString();
            //devices.AddView(label);

        }
    }
}