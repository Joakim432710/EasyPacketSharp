using System;
using System.IO;
using System.Text;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp.Generic
{
    public class MutablePacket : IMutablePacket
    {
        /// <summary>
        ///     <seealso cref="IPacket.Length"/>
        /// </summary>
        public ulong Length => (ulong)Writer.BaseStream.Length;

        /// <summary>
        ///     <seealso cref="IPacket.Position"/>
        /// </summary>
        public ulong Position => (ulong)Writer.BaseStream.Position;

        private BinaryWriter Writer { get; set; }
        private Encoding Encoding { get; }

        public bool Locked { get; private set; }

        public MutablePacket(Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;
            Writer = new BinaryWriter(new MemoryStream(), enc);
            Encoding = enc;
        }

        public void Focus(ulong at)
        {
            if (at > int.MaxValue) throw new NotImplementedException("This implementation does not support ulong lengths.");
            var buffer = new byte[Length];
            Buffer.BlockCopy(((MemoryStream)Writer.BaseStream).GetBuffer(), 0, buffer, 0, (int)at);
            Writer = new BinaryWriter(new MemoryStream(), Encoding);
        }

        public byte[] Lock()
        {
            if (Locked) throw new InvalidOperationException("The buffer mutation is already locked.");
            Locked = true;
            return ((MemoryStream) Writer.BaseStream).ToArray();
        }

        public void Unlock()
        {
            if (!Locked) throw new InvalidOperationException("The buffer mutation is already unlocked.");
            Locked = false;
        }

        public void Write(byte[] bytes)
        {
            while (Locked) { }
            Array.ForEach(bytes, WriteByte);
        }

        public void WriteInt(int val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteUInt(uint val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteLong(long val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteULong(ulong val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteShort(short val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteUShort(ushort val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteByte(byte val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteSByte(sbyte val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteFloat(float val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteDouble(double val)
        {
            while (Locked) { }
            Writer.Write(val);
        }

        public void WriteString(string str)
        {
            while (Locked) { }
            Writer.Write(str);
        }

        public void WriteStandardString(string str)
        {
            while (Locked) { }
            WriteUShort((ushort) str.Length);
            WriteString(str);
        }
    }
}