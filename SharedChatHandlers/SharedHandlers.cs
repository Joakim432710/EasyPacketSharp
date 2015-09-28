using EasyPacketSharp.Abstract;
using EasyPacketSharp.Generic;

namespace SharedChatHandlers
{
    public static class SharedHandlers
    {
        public static string GetMessage(IImmutablePacket pack)
        {
            return pack.ReadStandardString();
        }

        public static IPacket SendMessage(string s)
        {
            var pack = new MutablePacket();
            pack.WriteShort(2); //OPCode
            pack.WriteStandardString(s);
            return pack;
        }

        public static string GetName(IImmutablePacket pack)
        {
            return pack.ReadStandardString();
        }

        public static IPacket SendName(string s)
        {
            var pack = new MutablePacket();
            pack.WriteUShort((ushort)OperationCode.SetName); //OPCode
            pack.WriteStandardString(s);
            return pack;
        }
    }
}
