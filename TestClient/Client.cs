using EasyPacketSharp.Clients;

namespace ChatClient
{
    public class Client : TcpStreamClient
    {
        public string Name { get; set; }
    }
}
