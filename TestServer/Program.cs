using System;
using EasyPacketSharp.Abstract;
using EasyPacketSharp.Listeners;
using SharedChatHandlers;

namespace ChatServer
{
    public static class Program
    {
        static void Main()
        {
            IListener server = new TcpStreamListener(13337);
            server.Listen(500);
            server.Accept();

            server.OnConnection += s =>
            {
                Console.WriteLine($"Connection received from {s.Socket.RemoteEndPoint.ToString()}");
                s.SendPacket(SharedHandlers.SendName("Server"));
                var i = 0;
                s.OnPacket += (sock, p) =>
                {
                    var opCode = p.ReadUShort();
                    switch ((OperationCode)opCode)
                    {
                        case OperationCode.SendMessage:
                            Console.WriteLine($"Received: {SharedHandlers.GetMessage(p)}, Packet #{++i}");
                            break;
                        case OperationCode.SetName:
                            Console.WriteLine($"{sock.Socket.RemoteEndPoint.ToString()} changed name to {SharedHandlers.GetName(p)}");
                            break;
                    }

                };
            };

            server.OnDisconnection += s =>
            {
                Console.WriteLine($"{s.Socket.RemoteEndPoint.ToString()} disconnected");
            };

            while (true)
            {
                var str = Console.ReadLine();
                if (str == "stop" || str == "quit" || str == "shutdown")
                    break;
                server.SendPacket(SharedHandlers.SendMessage(str));
            }
        }
    }
}
