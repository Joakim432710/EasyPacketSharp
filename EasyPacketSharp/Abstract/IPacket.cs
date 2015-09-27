namespace EasyPacketSharp.Abstract
{
    public interface IPacket
    {
        /// <summary>
        ///     The current length of the packet
        /// </summary>
        ulong Length { get; }

        /// <summary>
        ///     The current position in the packet
        /// </summary>
        ulong Position { get; }

        /// <summary>
        ///     Focuses the packet at a certain part affecting all reads or writes to read from that position
        /// </summary>
        /// <param name="at">The position in the packet to focus at</param>
        void Focus(ulong at);
    }
}
