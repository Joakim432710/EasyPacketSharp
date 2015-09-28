using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp.Generic
{
    public class StandardSocket : ISocket
    {
        private byte[] Buffer { get; }

        public Socket Socket { get; }

        public event OnConnectionChangedMethod OnDisconnection;
        public event OnPacketMethod OnPacket;
        private bool Connected { get; set; }

        public StandardSocket(Socket s, ulong bufferSize = 1024, InitializeImmutablePacketMethod packetMethod = null)
        {
            Buffer = new byte[bufferSize];
            CreatePacketMethod = packetMethod;
            Socket = s;
            Connected = true;
        }

        public InitializeImmutablePacketMethod CreatePacketMethod { get; set; }
        public Encoding Encoding { get; set; }

        public void SendPacket(IPacket packet)
        {
            SendBytes(packet.Lock());
            if (packet.Locked)
                packet.Unlock();
        }

        private void SendBytes(byte[] bytes)
        {
            try { Socket.Send(bytes); }
            catch (InvalidOperationException) { SendBytes(bytes); }
        }

        /// <summary>
        ///     Acts as access method to start the receiving loop
        /// </summary>
        public void ReceivePackets() { Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceived, null); }
        private void OnReceived(IAsyncResult ar)
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

            try
            {
                Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceived, null);
            }
            catch (SocketException e)
            {
                if (e.Message.ToLowerInvariant().Contains("an existing connection was forcibly closed by the remote host"))
                {
                    OnDisconnection?.Invoke(this);
                    return;
                }
            }

            OnPacket?.Invoke(this, CreatePacketMethod?.Invoke(buf, Encoding));
        }

        public void Close()
        {
            if (!Connected) return;

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
            if (!Connected) return;
            if (disposing)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            Connected = false;
        }
    }
}
