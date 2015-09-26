using System.Net;

namespace EasyPacketSharp.Abstract
{
    public interface ISocket
    {
        void Write(string s);
        void WriteLine(string s);
        string ReadLine();
        string Read(int length);
    }
}
