using System.Text;

namespace EasyPacketSharp.Abstract
{
    public delegate void OnPacketMethod(IClient obj, IImmutablePacket packet);

    public delegate void OnDisconnectMethod(IClient obj);

    public delegate void OnConnectionChangedMethod(IClient obj);

    public interface ISocket
    {
        /// <summary>
        ///     Occurs whenever a connection has been established using the socket
        /// </summary>
        event OnConnectionChangedMethod OnConnection;

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
    }
}
