namespace EasyPacketSharp.Abstract
{
    public interface IMutablePacket : IPacket
    {
        /// <summary>
        ///     Writes a byte[] to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="bytes">The bytes to write to the packet</param>
        void Write(byte[] bytes);

        /// <summary>
        ///     Writes a standard integer to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">An integer to write to the packet</param>
        void WriteInt(int val);

        /// <summary>
        ///     Writes a standard unsigned integer to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">An unsigned integer to write to the packet</param>
        void WriteUInt(uint val);

        /// <summary>
        ///     Writes a standard long integer to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A long integer to write to the packet</param>
        void WriteLong(long val);

        /// <summary>
        ///     Writes a standard unsigned long integer to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">An unsigned long integer to write to the packet</param>
        void WriteULong(ulong val);

        /// <summary>
        ///     Writes a standard short to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A double to write to the packet</param>
        void WriteShort(short val);

        /// <summary>
        ///     Writes a standard unsigned short to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">An unsigned short to write to the packet</param>
        void WriteUShort(ushort val);

        /// <summary>
        ///     Writes a standard unsigned byte to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A byte to write to the packet</param>
        void WriteByte(byte val);

        /// <summary>
        ///     Writes a standard signed bytte to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A signed byte to write to the packet</param>
        void WriteSByte(sbyte val);

        /// <summary>
        ///     Writes a standard float to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A float to write to the packet</param>
        void WriteFloat(float val);

        /// <summary>
        ///     Writes a standard double to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="val">A double to write to the packet</param>
        void WriteDouble(double val);

        /// <summary>
        ///     Writes a standard string to the packet and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="str">The string to write to the packet</param>
        void WriteString(string str);

        /// <summary>
        ///     Writes a ushort to determine string length then Writes a standard string and moves the position to the next portion of the packet
        /// </summary>
        /// <param name="str">The string to write to the packet</param>
        void WriteStandardString(string str);
    }
}
