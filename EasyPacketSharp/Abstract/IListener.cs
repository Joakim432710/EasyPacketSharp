namespace EasyPacketSharp.Abstract
{
    public interface IListener : ISocket
    {
        void Bind(int port);
        void Listen(int backlog);
        void Accept();
    }
}
