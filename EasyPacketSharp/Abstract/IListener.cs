using System.Collections.Generic;

namespace EasyPacketSharp.Abstract
{
    public interface IListener : ISocket
    {
        /// <summary>
        ///     Occurs whenever a connection has been established using the socket
        /// </summary>
        event OnConnectionChangedMethod OnConnection;

        IList<ISocket> Clients { get; }
        InitializeSocketMethod CreateClientMethod { set; }

        void Bind(int port);
        void Listen(int backlog);
        void Accept();
    }
}
