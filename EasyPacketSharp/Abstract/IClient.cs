using System.Net;

namespace EasyPacketSharp.Abstract
{
    public interface IClient : ISocket
    {
        void Connect(IPAddress ip, int port);
    }
}
