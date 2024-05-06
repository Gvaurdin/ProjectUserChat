using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MessageProcessingLibrary;
using System.Runtime.InteropServices.ComTypes;
using System.Net.Http.Headers;

namespace ConsoleAppChatServer
{
    class ServerChat
    {
        static List<Client> _clientsConnections;
        static TcpListener _tcpListener;
        static IPHostEntry iPHostEntry;
        static int port;

        public ServerChat()
        {
            port = 49165;
            iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress networkIPAddress = iPHostEntry.AddressList.FirstOrDefault(
            ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
            && !IPAddress.IsLoopback(ip));
            _clientsConnections = new List<Client>();
            _tcpListener = new TcpListener(networkIPAddress, port);
        }

        public void Start()
        {
            _tcpListener.Start();
            Console.WriteLine("Server is run...");

            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                Console.WriteLine("Новый клиент подключен");

                // Обработка клиента в отдельном потоке
                HandleClient(tcpClient);
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                MemoryStream ms = new MemoryStream(buffer,0,bytesRead);

                PacketReader packetReader = new PacketReader(ms);

                MessageGarbage messageGarbage = packetReader.ReadMessage();
                // Ответ клиенту
                ResponseProcessing(messageGarbage,stream);
            }

            // Клиент отключился
            Console.WriteLine("Client offline");
            client.Close();
        }

        public static void ResponseProcessing(MessageGarbage messageGarbage, NetworkStream networkStream)
        {
            if (messageGarbage._opCode == OpCodes.Registration)
            {
                RegistrationMessage registrationMessage = (RegistrationMessage)messageGarbage._userMessage;
                bool userExists = _clientsConnections.Exists(user => user._username == registrationMessage._name);
                if(userExists) 
                {
                    byte[] response = Encoding.ASCII.GetBytes("Nok");
                    networkStream.Write(response, 0, response.Length);
                }
                else
                {
                    byte[] response = Encoding.ASCII.GetBytes("Ok");
                    networkStream.Write(response, 0, response.Length);
                }

            }
        }
    }
}
