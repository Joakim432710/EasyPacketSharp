using System;

namespace EasyPacketSharp.Exceptions
{
    public class InvalidListenerStateException : Exception
    {
        public InvalidListenerStateException() { }
        public InvalidListenerStateException(string msg) : base(msg) { }
        public InvalidListenerStateException(string msg, Exception inner) : base(msg, inner) { }
    }
}
