using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppUsersChat.Net
{
    class Server
    {
        static public IPHostEntry iPHostEntry { get; set; }

        static public IPAddress networkIPAddress { get; set; }

        static public int port { get; set; }

        public TcpClient _client { get; set; }
        public Server()
        {
            port = 49165;
            iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            networkIPAddress = iPHostEntry.AddressList.FirstOrDefault(
            ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
            && !IPAddress.IsLoopback(ip));
            _client = new TcpClient();
        }

        public void ConnectToServer(byte[] ResponseFromClient)
        {
            try
            {
                if (!_client.Connected)
                {
                    _client.Connect(networkIPAddress, port);
                    _client.GetStream().Write(ResponseFromClient, 0, ResponseFromClient.Length);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error : {ex}");
            }

        }

        public async Task<byte[]> WaitForResponseAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            byte[] response = new byte[bytesRead];
            Array.Copy(buffer, response, bytesRead);
            return response;
        }
    }
}
