using System;

namespace EasyPacketSharp.Exceptions
{
    public class ConnectErrorException : Exception
    {
        public ConnectErrorException() { }
        public ConnectErrorException(string msg) : base(msg) { }
        public ConnectErrorException(string msg, Exception inner) : base(msg, inner) { }
    }
}
