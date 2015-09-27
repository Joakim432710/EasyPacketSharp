using System;

namespace EasyPacketSharp.Exceptions
{
    class OutOfBoundsException : Exception
    {
        public OutOfBoundsException() { }
        public OutOfBoundsException(string msg) : base(msg) { }
        public OutOfBoundsException(string msg, Exception inner) : base(msg, inner) { }
    }
}
