namespace EasyPacketSharp
{
    public enum ConnectionState : byte
    {
        Uninitialized = 0,
        Initialized = 1,
        Connected = 2,
        Disconnected = 3
    }
}
