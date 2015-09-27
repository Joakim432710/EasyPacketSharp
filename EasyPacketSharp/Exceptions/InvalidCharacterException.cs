using System;

namespace EasyPacketSharp.Exceptions
{
    public class InvalidCharacterException : Exception
    {
        public InvalidCharacterException() { }
        public InvalidCharacterException(string msg) : base(msg) { }
        public InvalidCharacterException(string msg, Exception inner) : base(msg, inner) { }
    }
}
