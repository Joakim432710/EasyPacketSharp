using System.Text;

namespace EasyPacketSharp.Abstract
{
    public delegate IImmutablePacket InitializeImmutablePacketMethod(byte[] data, Encoding enc);
    public interface IImmutablePacket : IPacket
    {
        /// <summary>
        ///     Reads a byte[] of Length <paramref name="length"/> from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <param name="length">How many bytes to read from the packet</param>
        /// <returns>A byte[] of Length <paramref name="length"/> read at the current position</returns>
        byte[] Read(uint length);

        /// <summary>
        ///     Reads a standard integer from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>An integer read at the current position</returns>
        int ReadInt();

        /// <summary>
        ///     Reads a standard unsigned integer from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>An unsigned integer read at the current position</returns>
        uint ReadUInt();

        /// <summary>
        ///     Reads a standard long integer from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A long integer read at the current position</returns>
        long ReadLong();

        /// <summary>
        ///     Reads a standard unsigned long integer from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>An unsigned long integer read at the current position</returns>
        ulong ReadULong();

        /// <summary>
        ///     Reads a standard short from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A short read at the current position</returns>
        short ReadShort();

        /// <summary>
        ///     Reads a standard unsigned short from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>An unsigned short read at the current position</returns>
        ushort ReadUShort();

        /// <summary>
        ///     Reads a standard unsigned byte from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>An unsigned byte read at the current position</returns>
        byte ReadByte();

        /// <summary>
        ///     Reads a standard signed bytte from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A signed short read at the current position</returns>
        sbyte ReadSByte();

        /// <summary>
        ///     Reads a standard float from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A float read at the current position</returns>
        float ReadFloat();

        /// <summary>
        ///     Reads a standard double from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A double read at the current position</returns>
        double ReadDouble();

        /// <summary>
        ///     Reads a standard string of length <paramref name="length"/> from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <param name="length"></param>
        /// <returns>A string of length <paramref name="length"/> read at the current position</returns>
        string ReadString(ulong length);

        /// <summary>
        ///     Reads a null-terminated string from the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A null-terminated string read at the current position</returns>
        string ReadString();

        /// <summary>
        ///     Reads a ushort to determine string length then reads a standard string using that ushort as length and moves the position to the next portion of the packet
        /// </summary>
        /// <exception cref="Exceptions.OutOfBoundsException">Thrown when trying to read more than <see cref="IPacket.Length"/> - <see cref="IPacket.Position"/> bytes</exception>
        /// <returns>A string read at the current position, see summary</returns>
        string ReadStandardString();
    }
}
