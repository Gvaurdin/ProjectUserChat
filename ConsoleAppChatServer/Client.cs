using MessageProcessingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppChatServer
{
    class Client
    {
        public string _username { get; set; }
        public Guid _uID { get; set; }
        public string  _password { get; set; }
        public TcpClient _tcpClient { get; set; }

        PacketReader packetReader;

        public Client(TcpClient tcpClient)
        {
            _uID = Guid.NewGuid();
            _tcpClient = tcpClient;
        }

        public void SetPropertes(string name, string password)
        {
            _username = name;
            _password = password;
        }

    }
}
