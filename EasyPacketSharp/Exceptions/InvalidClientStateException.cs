using System;

namespace EasyPacketSharp.Exceptions
{
    public class InvalidClientStateException : Exception
    {
        public InvalidClientStateException() { }
        public InvalidClientStateException(string msg) : base(msg) { }
        public InvalidClientStateException(string msg, Exception inner) : base(msg, inner) { }
    }
}
