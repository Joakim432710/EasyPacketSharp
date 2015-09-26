using System;
using System.Net;
using System.Net.Sockets;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp
{
    public class TcpStreamClient : IClient
    {
        private byte[] Buffer { get; }
        private Socket Socket { get; }
        public TcpStreamClient(EndPoint ep)
        {
            Buffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public TcpStreamClient(IPAddress ip, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.BeginConnect(new IPEndPoint(ip, port), OnConnected, null)
        }

        public static ISocket Create(EndPoint ep)
        {
            throw new System.NotImplementedException();
        }

        public ISocket Create(IPAddress ip, int port)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string s)
        {
            throw new System.NotImplementedException();
        }

        public void WriteLine(string s)
        {
            throw new System.NotImplementedException();
        }

        public string ReadLine()
        {
            throw new System.NotImplementedException();
        }

        public string Read(int length)
        {
            throw new System.NotImplementedException();
        }
        private void OnConnected(IAsyncResult ar)
        {
            Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceive, null);
        }

        private void OnReceive(IAsyncResult ar)
        {
            SocketError err;
            var bytesReceived = Socket.EndReceive(ar, out err);

            switch (err)
            {
                case SocketError.Success:
                    break;
                case SocketError.Disconnecting:
                    //Handle
                    return;
            }

            var buf = new byte[bytesReceived];
            System.Buffer.BlockCopy(Buffer, 0, buf, 0, buf.Length);
        }
    }
}
