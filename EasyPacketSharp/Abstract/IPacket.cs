namespace EasyPacketSharp.Abstract
{
    /// <summary>
    ///     An interface that describes an internal packet
    ///     There are no write only packets because to be able to transmit data one must be able to read from it as well
    /// </summary>
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

        /// <summary>
        ///     Returns the packet data     
        /// </summary>
        /// <remarks>
        ///     Locks the packet from any mutations first 
        ///     After locking the implementer should either: 
        ///         Return a copy of the entire packet, and then unlock the packet
        ///     or
        ///         Return the entire packet, and keep the immutable state until Unlock is explicitly called by a user
        /// </remarks>
        /// <returns>The packet data</returns>
        byte[] Lock();

        /// <summary>
        ///     Unlocks the buffer and permits mutations again
        /// </summary>
        void Unlock();
    }
}
