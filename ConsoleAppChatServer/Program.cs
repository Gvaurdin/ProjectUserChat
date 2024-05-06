using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppChatServer
{
    internal class Program
    {
        static List<Client> _users;
        static TcpListener _tcpListener;
        static IPHostEntry iPHostEntry;
        static int port;
        static void Main(string[] args)
        {
            ServerChat serverChat = new ServerChat();
            serverChat.Start();
        }
    }
}
