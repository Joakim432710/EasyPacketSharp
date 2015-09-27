using System.Net.Sockets;
using System.Text;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp.Generic
{
    public class StandardSocket : ISocket
    {
        public Socket Socket { get; }
        public event OnConnectionChangedMethod OnConnection;
        public event OnConnectionChangedMethod OnDisconnection;
        public event OnPacketMethod OnPacket;

        public StandardSocket(Socket s)
        {
            Socket = s;
        }

        public InitializeImmutablePacketMethod CreatePacketMethod { get; set; }
        public Encoding Encoding { get; set; }

        public void SendPacket(IPacket packet)
        {

        }
    }
}
