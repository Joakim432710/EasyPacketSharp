namespace EasyPacketSharp.Abstract
{
    public interface IListener : ISocket
    {
        void Listen(int backlog);
        void Accept();
    }
}
