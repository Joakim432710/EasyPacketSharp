using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EasyPacketSharp.Abstract;
using EasyPacketSharp.Exceptions;
using EasyPacketSharp.Generic;

namespace EasyPacketSharp.Clients
{
    public class TcpStreamClient : IClient
    {
        private ConnectionState State { get; set; }
        private byte[] Buffer { get; }

        public TcpStreamClient(EndPoint ep, long bufferSize = 1024, InitializeImmutablePacketMethod packetMethod = null)
        {
            Buffer = new byte[bufferSize];
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            State = ConnectionState.Initialized;
            if (packetMethod == null)
                CreatePacketMethod = CreateStandardPacket;
            Connect(ep);
        }
        public TcpStreamClient(IPAddress ip, int port, long bufferSize = 1024) : this(new IPEndPoint(ip, port), bufferSize) { }


        private IImmutablePacket CreateStandardPacket(byte[] bytes, Encoding enc)
        {
            return new ImmutablePacket(bytes, enc);
        }

        #region ISocket Implementation

        public Socket Socket { get; }

        public Encoding Encoding { get; set; }

        public InitializeImmutablePacketMethod CreatePacketMethod { private get; set; }

        public event OnConnectionChangedMethod OnConnection;

        public event OnConnectionChangedMethod OnDisconnection;

        public event OnPacketMethod OnPacket;

        public void SendPacket(IPacket p)
        {
            Socket.Send(p.Lock());
            if(p.Locked)
                p.Unlock();
        }

        #endregion ISocket Implementation

        #region IClient Implementation

        public void Connect(IPAddress host, ushort port) { Connect(new IPEndPoint(host, port)); }
        public void Connect(ushort port, IPAddress host) { Connect(new IPEndPoint(host, port)); }

        public void Connect(ushort port, string host)
        {
            var hostEntries = Dns.GetHostEntry(host);
            if(hostEntries.AddressList.Length < 1) throw new ParseHostException($"Could not parse host {host}");
            Connect(hostEntries.AddressList[0], port);
        }

        public void Connect(string host)
        {
            if (TryConnectProtocol(host)) return;
            if (TryConnectSeparate(host)) return;
            throw new ParseHostException($"Could not parse host because either port, protocol or ip could not be parsed from {host}" + Environment.NewLine + "See remarks for additional formatting information");
        }
        public void Connect(EndPoint ep)
        {
            if (State != ConnectionState.Initialized) throw new ConnectErrorException($"Cannot connect from state {State}");
            Socket.BeginConnect(ep, BeginConnectCallback, null);
        }
        public void Connect(ushort port, string host, uint numberBase) //TODO: Handle IPV6
        {
            Connect(port, host.ParseIPFromBase(numberBase));
            if (!TryConnectSeparate(host, numberBase))
                throw new ParseHostException($"Could not parse host because either port, protocol or ip could not be parsed from {host}" + Environment.NewLine + "See remarks for additional formatting information");
        }

        public void Connect(string host, uint numberBase) //TODO: Handle IPV6
        {
            if (TryConnectProtocol(host, numberBase)) return;
            if (TryConnectSeparate(host, numberBase)) return;

            throw new ParseHostException($"Could not parse host because either port, protocol or ip could not be parsed from {host}" + Environment.NewLine + "See remarks for additional formatting information");
        }

        #region Protocols

        private void ConnectProtocol(string protocol, string ip)
        {
            if(!TryConnectProtocol(protocol, ip)) throw new ParseHostException($"Invalid protocol '{protocol}' supplied ");
        }

        private bool TryConnectProtocol(string protocol, string ip)
        {
            ushort? port = null;
            switch (protocol.ToLowerInvariant())
            {
                case "ftp":
                    port = 21;
                    break;

                case "scp":
                case "sftp":
                case "ssh":
                    port = 22;
                    break;
                case "telnet":
                    port = 23;
                    break;

                case "smtp":
                    port = 25;
                    break;

                case "time":
                    port = 37;
                    break;

                case "whois":
                    port = 43;
                    break;

                case "dns":
                    port = 53;
                    break;

                case "rap":
                    port = 56;
                    break;

                case "dhcp":
                    port = 67; //See: https://en.wikipedia.org/wiki/Dynamic_Host_Configuration_Protocol Client uses 67 to connect to server, Server uses 68 to connect to client
                    break;

                case "tftp":
                    port = 69;
                    break;

                case "http":
                    port = 80;
                    break;

                case "pop2":
                    port = 109;
                    break;

                case "pop3":
                    port = 110;
                    break;

                case "nntp":
                    port = 119;
                    break;

                case "ntp":
                    port = 123;
                    break;

                case "imap":
                    port = 143;
                    break;

                case "bftp":
                    port = 152;
                    break;

                case "sgmp":
                    port = 153;
                    break;

                case "irc":
                    port = 194;
                    break;

                case "https":
                    port = 443;
                    break;

                case "talk":
                    port = 517;
                    break;

                case "ntalk":
                    port = 518;
                    break;

                case "rpc":
                    port = 530;
                    break;

                case "aol":
                    port = 531;
                    break;

                case "uucp":
                    port = 540;
                    break;

                case "afp":
                    port = 548;
                    break;

                case "nntps":
                    port = 563;
                    break;

                case "ipp":
                    port = 631;
                    break;

                case "mms":
                    port = 651;
                    break;

                case "mmp":
                    port = 654;
                    break;

                case "ftps":
                    port = 990;
                    break;

                case "telnets":
                    port = 992;
                    break;

                case "ircs":
                    port = 994;
                    break;

                case "pop3s":
                    port = 995;
                    break;

                case "multiplayer":
                    port = 55650;
                    break;

                case "chat":
                    port = 13337;
                    break;
            }
            if (!port.HasValue) return false;
            Connect(port.Value, ip);
            return true;
        }

        private bool TryConnectProtocol(string host)
        {
            var split = host.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 2) throw new ParseHostException($"Invalid format supplied, detected more than one :// in the string {host}");
            if (split.Length != 2) return false;

            ConnectProtocol(split[0].Trim(), split[1].Trim());
            return true;
        }
        
        private bool TryConnectProtocol(string host, uint numberBase)
        {
            var split = host.Split(StringExtension.ProtocolSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 2) throw new ParseHostException($"Invalid format supplied, detected more than one :// in the string {host}");
            if (split.Length != 2) return false;
            var ip = split[1].ParseIPFromBase(numberBase);
            ConnectProtocol(split[0], ip);
            return true;
        }

        #endregion

        #region Separators

        private bool TryConnectSeparate(string host)
        {
            ushort? port = null;
            var ip = string.Empty;

            var split = host.Split(StringExtension.HostPortSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in split)
            {
                var final = val.Trim();
                var segments = final.Split(StringExtension.HostPortSeparators);
                if (segments.Length < 1)
                    port = ushort.Parse(final);
                else
                    ip = final;
            }
            if (!port.HasValue) return false;
            Connect(port.Value, ip);
            return true;
        }

        private bool TryConnectSeparate(string host, uint numberBase)
        {
            ushort? port = null;
            var ip = string.Empty;

            var split = host.Split(StringExtension.HostPortSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in split)
            {
                var final = val.Trim();
                var segments = final.Split(StringExtension.HostPortSeparators);
                if (segments.Length < 1)
                    port = ushort.Parse(final);
                else
                    ip = final.ParseIPFromBase(numberBase);
            }
            if (!port.HasValue) return false;
            Connect(port.Value, ip);
            return true;
        }

        #endregion

        #endregion IClient Implementation

        #region Private Async Implementation Pattern
        private void BeginConnectCallback(IAsyncResult ar)
        {
            if(!Socket.Connected) throw new ConnectErrorException("The internal socket refused the connection.");
            State = ConnectionState.Connected;
            OnConnection?.Invoke(this);
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
                    OnDisconnection?.Invoke(this);
                    State = ConnectionState.Disconnected;
                    return;
            }

            var buf = new byte[bytesReceived];
            System.Buffer.BlockCopy(Buffer, 0, buf, 0, buf.Length);
            Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, OnReceive, null); //Start new receive as fast as possible

            if (CreatePacketMethod == null) throw new InvalidClientStateException("The user has made runtime edits to the client that has put it in an invalid state. Make sure CreatePacketMethod always has a value");
            
            OnPacket?.Invoke(this, CreatePacketMethod(buf, Encoding));
        }
        #endregion Private Async Implementation Pattern
    }
}
