﻿using System;
using System.Net.Sockets;
using System.Text;

namespace EasyPacketSharp.Abstract
{
    public delegate void OnPacketMethod(ISocket obj, IImmutablePacket packet);

    public delegate void OnDisconnectMethod(ISocket obj);

    public delegate void OnConnectionChangedMethod(ISocket obj);

    public delegate ISocket InitializeSocketMethod(Socket s, ulong bufferSize, InitializeImmutablePacketMethod packetMethod);

    public interface ISocket : IDisposable
    {
        Socket Socket { get; }

        /// <summary>
        ///     Occurs whenever a connection has been disconnected on the socket
        /// </summary>
        event OnConnectionChangedMethod OnDisconnection;

        /// <summary>
        ///     Occurs whenever a packet has been sent to the socket
        /// </summary>
        event OnPacketMethod OnPacket;

        /// <summary>
        ///     The method for which the socket should create packets with
        /// </summary>
        InitializeImmutablePacketMethod CreatePacketMethod { set; }

        /// <summary>
        ///     The encoding for which the socket should interpret string based data with
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        ///     Writes a packet to the connection
        /// </summary>
        /// <param name="packet">The packet to write</param>
        void SendPacket(IPacket packet);

        /// <summary>
        ///     Starts a receive-loop that listens for incoming packets and forwards them to event handles
        /// </summary>
        void ReceivePackets();

        /// <summary>
        ///     Closes this socket allowing no further comminication with or using the socket
        /// </summary>
        /// <remarks>
        ///     Should dispose the instance
        /// </remarks>
        void Close();
    }
}
