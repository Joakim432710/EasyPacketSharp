namespace EasyPacketSharp.Abstract
{
    public interface IListener : ISocket
    {
        InitializeSocketMethod CreateClientMethod { set; }

        void Bind(int port);
        void Listen(int backlog);
        void Accept();
    }
}
