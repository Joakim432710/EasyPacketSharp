using System;
using System.Net;
using System.Net.Sockets;

namespace EasyPacketSharp.Abstract
{
    public class TcpStreamListener : IListener
    {
        private byte[] SharedBuffer { get; }
        private Socket Socket { get; }

        public TcpStreamListener(EndPoint ep)
        {
            SharedBuffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        public ISocket Create(EndPoint ep)
        {
            return new TcpStreamListener(ep);
        }

        public ISocket Create(IPAddress ip, int port)
        {
            return new TcpStreamListener(new IPEndPoint(ip, port));
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

        public void Listen(int backlog)
        {
            Socket.Listen(backlog);
        }

        public void Accept()
        {
            Socket.BeginAccept(OnAccepted, null);
        }

        private void OnAccepted(IAsyncResult ar)
        {
            var sock = Socket.EndAccept(ar);
            var buf = new byte[1024];
            sock.BeginReceive(SharedBuffer, 0, SharedBuffer.Length, SocketFlags.None, OnReceived, sock);
            Accept();
        }

        private void OnReceived(IAsyncResult ar)
        {
            var sock = ar.AsyncState as Socket;
            if(sock == null) throw new ArgumentException("Invalid result passed to OnReceived. User-defined object was either null or not of type Socket.", nameof(ar));
            SocketError err;
            var bytesReceived = sock.EndReceive(ar, out err);

            switch (err)
            {
                case SocketError.Success:
                    break;
                case SocketError.Disconnecting:
                    //Handle
                    return;
            }

            var buf = new byte[bytesReceived];
            Buffer.BlockCopy(SharedBuffer, 0, buf, 0, buf.Length);
            sock.BeginReceive(SharedBuffer, 0, SharedBuffer.Length, SocketFlags.None, OnReceived, sock);
        }
    }
}
