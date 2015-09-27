using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using EasyPacketSharp.Abstract;
using EasyPacketSharp.Clients;
using EasyPacketSharp.Exceptions;
using EasyPacketSharp.Generic;

namespace EasyPacketSharp.Listeners
{
    public class TcpStreamListener : IListener
    {
        private byte[] SharedBuffer { get; }
        public Socket Socket { get; }

        public TcpStreamListener(InitializeSocketMethod clientMethod = null)
        {
            SharedBuffer = new byte[1024];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (clientMethod == null)
                CreateClientMethod = CreateStandardSocket;
        }

        private ISocket CreateStandardSocket(Socket s)
        {
            return new StandardSocket(s);
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

        public InitializeSocketMethod CreateClientMethod { private get; set; }

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
            if (CreateClientMethod == null)
                throw new InvalidListenerStateException("The user has made runtime edits to the client that has put it in an invalid state. Make sure CreateClientMethod always has a value");
            var sock = CreateClientMethod(Socket.EndAccept(ar));
            sock.Socket.BeginReceive(SharedBuffer, 0, SharedBuffer.Length, SocketFlags.None, OnReceived, sock);
            Accept();
        }

        private void OnReceived(IAsyncResult ar)
        {
            var sock = ar.AsyncState as ISocket;
            if (sock == null) throw new ArgumentException("Invalid result passed to OnReceived. User-defined object was either null or not of type Socket.", nameof(ar));
            SocketError err;
            var bytesReceived = sock.Socket.EndReceive(ar, out err);

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
            sock.Socket.BeginReceive(SharedBuffer, 0, SharedBuffer.Length, SocketFlags.None, OnReceived, sock);

            OnPacket?.Invoke(sock, CreatePacketMethod?.Invoke(buf, Encoding));
        }

        #endregion IListener Implementation
    }
}