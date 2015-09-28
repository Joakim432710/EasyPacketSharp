using System.Net;

namespace EasyPacketSharp.Abstract
{
    public interface IClient : ISocket
    {
        /// <summary>
        ///     Occurs whenever a connection has been established using the socket
        /// </summary>
        event OnConnectionChangedMethod OnConnection;

        /// <summary>
        ///     Opens an IP connection to <paramref name="host"/> using port <paramref name="port"/>
        ///     <seealso cref="Connect(ushort, IPAddress)"/>
        ///     <seealso cref="Connect(EndPoint)"/>
        /// </summary>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="host">The host to connect to</param>
        /// <param name="port">The port as an unsigned 16-bit integer</param>
        void Connect(IPAddress host, ushort port);

        /// <summary>
        ///     Opens an IP connection to <paramref name="host"/> using port <paramref name="port"/>
        ///     <seealso cref="Connect(IPAddress, ushort)"/>
        ///     <seealso cref="Connect(EndPoint)"/>
        /// </summary>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="host">The host to connect to</param>
        /// <param name="port">The port as an unsigned 16-bit integer</param>
        void Connect(ushort port, IPAddress host);

        /// <summary>
        ///     Opens a connection to <paramref name="ep"/>
        ///     <seealso cref="Connect(IPAddress, ushort)"/>
        ///     <seealso cref="Connect(ushort, IPAddress)"/>
        /// </summary>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="ep">EndPoint determining the socket's connection destination</param>
        void Connect(EndPoint ep);

        /// <summary>
        ///     Opens an IP Connection to <paramref name="host"/> using port <paramref name="port"/>
        ///     <seealso cref="Connect(ushort, string, uint)"/>
        /// </summary>
        /// <remarks>
        ///     The host is not limited to IPv4, any IP Address will do
        /// </remarks>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="port">The port as an unsigned 16-bit integer</param>
        /// <param name="host">The host to connect to parsed by base 10</param>
        void Connect(ushort port, string host);

        /// <summary>
        ///     Opens an IP Connection to <paramref name="host"/> using port <paramref name="port"/>.
        ///     <seealso cref="Connect(ushort, string)"/>
        /// </summary>
        /// <remarks>
        ///     The host is not limited to IPv4, any IP Address will do
        /// 
        ///     Conversion from numberBase to a binary address should occur
        ///     To do this conversion the following scheme is used:
        ///     Base  2 - 16: Hexadecimal Scheme (0-9 then A-F)
        ///     Base 17 - 32: RFC4648 Base32 Scheme
        ///     Base 33 - 64: RFC4648 Base64 Scheme
        ///     <see cref="http://tools.ietf.org/html/rfc4648"/>
        /// </remarks>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="port">The port as an unsigned 16-bit integer</param>
        /// <param name="host">The host to connect to parsed by base <paramref name="numberBase"/></param>
        /// <param name="numberBase">The base to parse <paramref name="host"/> with</param>
        void Connect(ushort port, string host, uint numberBase);

        /// <summary>
        ///     Opens an IP Connection to <paramref name="ip"/>
        ///     <seealso cref="Connect(string)"/>
        /// </summary>
        /// <remarks>

        ///     Ip may be supplied in four possible formats:
        ///         "port, ip"        : "80, 10.10.110.27" (Space is optional)
        ///         "host, ip"        : "10.10.110.27, 80" (Space is optional)
        ///         "host:ip"         : "10.10.110.27:80"
        ///         "protocol://ip"   : "http://10.10.110.27"
        ///     
        ///     The ip is not limited to IPv4, any IP Address will do
        /// 
        ///     Conversion from numberBase to a binary address should occur
        ///     To do this conversion the following scheme is used:
        ///     Base  2 - 16: Hexadecimal Scheme (0-9 then A-F)
        ///     Base 17 - 32: RFC4648 Base32 Scheme
        ///     Base 33 - 64: RFC4648 Base64 Scheme
        ///     <see cref="http://tools.ietf.org/html/rfc4648"/>
        /// </remarks>
        /// <exception cref="Exceptions.ParseHostException">
        ///     Thrown if the input does not follow one of the known formats 
        ///     For more information see remarks
        /// </exception>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="ip">The host to connect to parsed by base <paramref name="numberBase"/>, for formatting see remarks</param>
        /// <param name="numberBase">The base to parse <paramref name="ip"/> with, for standardization see remarks</param>
        void Connect(string ip, uint numberBase);

        /// <summary>
        ///     Opens an IP Connection to <paramref name="host"/>
        ///     <seealso cref="Connect(string, uint)"/>
        /// </summary>
        /// <remarks>
        ///     Host may be supplied in four possible formats:
        ///         "port, host"        : "80, 10.10.110.27"
        ///         "host, port"        : "10.10.110.27, 80"
        ///         "host:port"         : "10.10.110.27:80"
        ///         "protocol://host"   : "http://google.com"
        ///     
        ///     The host is not limited to IPv4, any IP Address or hostname will do
        /// </remarks>
        /// <exception cref="Exceptions.ParseHostException">
        ///     Thrown if the input does not follow one of the known formats 
        ///     For more information see remarks
        /// </exception>
        /// <exception cref="Exceptions.ConnectErrorException">Thrown if the socket refuses to open the connection</exception>
        /// <param name="host">The host to connect to parsed by base 10, for formatting see remarks</param>
        void Connect(string host);
    }
}
