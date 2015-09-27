using System;

namespace EasyPacketSharp.Exceptions
{
    public class ParseHostException : Exception
    {
        public ParseHostException() { }
        public ParseHostException(string msg) : base(msg) { }
        public ParseHostException(string msg, Exception inner) : base(msg, inner) { }
    }
}
