using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasyPacketSharp.Abstract;
using EasyPacketSharp.Exceptions;
using EasyPacketSharp.Generic;

namespace EasyPacketSharp.Listeners
{
    public class TcpStreamListener : IListener
    {
        private ulong BufferSize { get; }

        public TcpStreamListener(ulong bufferSize = 1024, InitializeSocketMethod clientMethod = null, InitializeImmutablePacketMethod packetMethod = null)
        {
            BufferSize = bufferSize;
            if (clientMethod == null)
                CreateClientMethod = CreateStandardSocket;
            if (packetMethod == null)
                CreatePacketMethod = CreateStandardPacket;
            Clients = new List<ISocket>();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public TcpStreamListener(ushort port, ulong bufferSize = 1024, InitializeSocketMethod clientMethod = null, InitializeImmutablePacketMethod packetMethod = null) : this(bufferSize, clientMethod, packetMethod)
        {
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        private static ISocket CreateStandardSocket(Socket s, ulong bufferSize, InitializeImmutablePacketMethod packetMethod)
        {
            return new StandardSocket(s, bufferSize, packetMethod);
        }

        private static IImmutablePacket CreateStandardPacket(byte[] bytes, Encoding enc)
        {
            return new ImmutablePacket(bytes, enc);
        }

        #region ISocket Implementation

        /// <summary>
        ///     Invoked whenever anything successfully connects to this listener
        /// </summary>
        public event OnConnectionChangedMethod OnConnection;

        /// <summary>
        ///     Invoked whenever anything that was connected to this listener disconnects
        /// </summary>
        public event OnConnectionChangedMethod OnDisconnection;

        /// <summary>
        ///     Invokes whenever anything connected to this listener receives a packet
        /// </summary>
        public event OnPacketMethod OnPacket;

        /// <summary>
        ///     The listener socket used for receiving connections
        /// </summary>
        /// <remarks>
        ///     Doesn't actually have an in/out stream to read or write to
        ///     Simply acts as a connection receiver and then dispatches the connection to a new ISocket instance
        /// </remarks>
        public Socket Socket { get; }

        /// <summary>
        ///     The packet creation method used when receiving packets
        /// </summary>
        public InitializeImmutablePacketMethod CreatePacketMethod { get; set; }

        /// <summary>
        ///     The encoding to read character based data with
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        ///     Sends a packet to all clients of this socket
        /// </summary>
        /// <param name="packet">A packet to send to the clients</param>
        public void SendPacket(IPacket packet)
        {
            foreach (var client in Clients)
                client.SendPacket(packet);
        }

        void ISocket.ReceivePackets()
        {
            if (Socket.IsBound)
                Listen(500);
            //Implicitly already does this when you 'listen'
        }

        /// <summary>
        ///     Closes the listener and all connections to it
        /// </summary>
        public void Close()
        {
            if (!Socket.IsBound) return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!Socket.IsBound) return;
            if (disposing)
            {
                foreach (var client in Clients)
                {
                    client.Close();
                }
                Socket.Dispose();
            }
        }

        #endregion ISocket Implementation 

        #region IListener Implementation

        public IList<ISocket> Clients { get; set; }
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
            var sock = CreateClientMethod(Socket.EndAccept(ar), BufferSize, CreatePacketMethod);
            Clients.Add(sock);

            sock.OnDisconnection += s =>
            {
                Clients.Remove(s);
                OnDisconnection?.Invoke(s);
            };
            sock.OnPacket += (s, pack) => { OnPacket?.Invoke(s, pack); };

            OnConnection?.Invoke(sock);
            sock.ReceivePackets();
            Accept();
        }

        #endregion IListener Implementation
    }
}