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
            Locked = true;
            Array.ForEach(bytes, Writer.Write);
            Locked = false;
        }

        public void WriteInt(int val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteUInt(uint val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteLong(long val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteULong(ulong val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteShort(short val)
        {
            while (Locked) { }
            Writer.Write(val);
            Locked = false;
        }

        public void WriteUShort(ushort val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteByte(byte val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteSByte(sbyte val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteFloat(float val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteDouble(double val)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(val);
            Locked = false;
        }

        public void WriteString(string str)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write(str);
            Locked = false;
        }

        public void WriteStandardString(string str)
        {
            while (Locked) { }
            Locked = true;
            Writer.Write((ushort) str.Length);
            Writer.Write(Encoding.GetBytes(str));
            Locked = false;
        }
    }
}