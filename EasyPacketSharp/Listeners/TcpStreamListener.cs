using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp.Listeners
{
    public class TcpStreamListener : IListener
    {
        private byte[] SharedBuffer { get; }
        private Socket Socket { get; }

        public TcpStreamListener()
        {
            SharedBuffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public TcpStreamListener(int port)
        {
            SharedBuffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        #region ISocket Implementation

        public event OnConnectionChangedMethod OnConnection;

        public event OnConnectionChangedMethod OnDisconnection;

        public event OnPacketMethod OnPacket;

        public InitializeImmutablePacketMethod CreatePacketMethod { get; set; }

        public Encoding Encoding { get; set; }

        public void SendPacket(IPacket packet)
        {
            throw new NotImplementedException();
        }

        #endregion ISocket Implementation 

        #region IListener Implementation

        public void Bind(int port)
        {
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
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
            sock.BeginReceive(SharedBuffer, 0, SharedBuffer.Length, SocketFlags.None, OnReceived, sock);
            Accept();
        }

        private void OnReceived(IAsyncResult ar)
        {
            var sock = ar.AsyncState as Socket;
            if (sock == null) throw new ArgumentException("Invalid result passed to OnReceived. User-defined object was either null or not of type Socket.", nameof(ar));
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

        #endregion IListener Implementation
    }
}