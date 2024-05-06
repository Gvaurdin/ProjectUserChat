using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingLibrary
{
    public enum OpCodes
    {
        General = 1,
        Group = 2,
        Private = 3,
        Registration = 4,
        Login = 5
    }
    [Serializable]
    public class MessageGarbage
    {
        public OpCodes _opCode { get; set; }

        public UserMessage _userMessage { get; set; }

        public MessageGarbage(UserMessage userMessage)
        {

            if (userMessage is GeneralMessage)
            {
                _userMessage = (GeneralMessage)userMessage;
                _opCode = OpCodes.General;
            }
            else if (userMessage is GroupMessage)
            {
                _userMessage = (GroupMessage)userMessage;
                _opCode = OpCodes.Group;
            }
            else if(userMessage is PrivateMessage)
            {
                _userMessage = (PrivateMessage)userMessage;
                _opCode = OpCodes.Private;
            }
            else if(userMessage is RegistrationMessage)
            {
                _userMessage = (RegistrationMessage)userMessage;
                _opCode = OpCodes.Registration;
            }
            else if(userMessage is LoginMessage)
            {
                _userMessage = (LoginMessage)userMessage;
                _opCode = OpCodes.Login;
            }
        }
    }
    [Serializable]
    public class UserMessage
    {
        public string _message { get; set; }
        public UserMessage() { }

        public UserMessage(string message)
        {
            _message = message;
        }
    }
    [Serializable]
    public class GeneralMessage : UserMessage
    {
        public string _senderName { get; set; }

        public GeneralMessage() { }

        public GeneralMessage(string senderName, string message) : base(message)
        {
            _senderName = senderName;
        }
    }
    [Serializable]
    public class PrivateMessage : UserMessage 
    {
        public string _senderName { get; set;}
        public string _recipientName { get; set; }

        public PrivateMessage() { } 

        public PrivateMessage(string senderName, string recipientName, string message) : base(message)
        {
            _senderName = senderName;
            _recipientName = recipientName;
        }
    }
    [Serializable]
    public class GroupMessage : UserMessage
    {
        public string _senderName { get; set; }
        public string  groupName { get; set; }

        public GroupMessage() { }

        public GroupMessage(string senderName, string groupName, string message) : base(message)
        {
            _senderName = senderName;
            this.groupName = groupName;
        }
    }
    [Serializable]
    public class RegistrationMessage : UserMessage
    {
        public string _name { get; set; }
        public string  _password { get; set; }

        public RegistrationMessage() { }

        public RegistrationMessage(string name, string password, string message) : base(message)
        {
            _name = name;
            _password = password;
        }

        public static explicit operator RegistrationMessage(MessageGarbage v)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public class LoginMessage : UserMessage
    {
        public string _name { get; set; }
        public string _password { get; set; }

        public LoginMessage() { }

        public LoginMessage(string name, string password, string message) : base(message)
        {
            _name = name;
            _password = password;
        }
    }
    [Serializable]
    public class PacketReader
    {
        private MemoryStream _ns;
        public PacketReader(MemoryStream ns)
        {
            _ns = ns;
        }

        public MessageGarbage ReadMessage()
        {
            _ns.Position = 0;
            byte opcode = (byte)_ns.ReadByte();
            byte[] packetData = new byte[_ns.Length - 1];
            _ns.Read(packetData,0, packetData.Length);
            return DeserializePacket(packetData);
        }

        private static MessageGarbage DeserializePacket(byte[] packetData)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(packetData))
            {
                return (MessageGarbage)binaryFormatter.Deserialize(ms);
            }
        }

        [Serializable]
        public class PacketBuilder
        {
            MemoryStream _ms;

            public PacketBuilder()
            {
                _ms = new MemoryStream();
            }

            public void WriteOpCode(OpCodes opCode)
            {
                byte byteOpCode = (byte)opCode;
                _ms.WriteByte(byteOpCode);
            }

            public void WriteMessageGarbage(MessageGarbage messageGarbage)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(_ms,messageGarbage);
            }

            public byte[] GetPacketBytes()
            {
                return _ms.ToArray();
            }
        }
    }
}
