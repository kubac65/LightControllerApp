using System;
using System.Net;
using System.Net.Sockets;

namespace LightControl.Network
{
    public delegate void DeviceDiscovered(object sender, DeviceDiscoveredEventArgs eventArgs);

    public class DeviceDiscoveredEventArgs : EventArgs
    {
        public IPAddress Address { get; set; }
        public byte[] Mac { get; set; }
    }

    public class DiscoveryService : IDisposable
    {
        private static int Port = 54545;

        private UdpClient _udp = new UdpClient();
        private IPEndPoint _src = new IPEndPoint(IPAddress.Any, Port);

        public event DeviceDiscovered DeviceDiscovered;

        public DiscoveryService()
        {
            _udp.Client.Bind(_src);
        }

        public void BeginReceive()
        {
            _udp.BeginReceive(new AsyncCallback(ReceiveMessage), this);
        }

        public void Dispose()
        {
            _udp.Dispose();
        }

        private void ReceiveMessage(IAsyncResult ar)
        {
            byte[] receivedMessage = _udp.EndReceive(ar, ref _src);
            DeviceDiscovered?.Invoke(this, new DeviceDiscoveredEventArgs()
            {
                Address = _src.Address,
                Mac = receivedMessage
            });
            BeginReceive();
        }
    }
}