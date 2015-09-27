using System;
using System.IO;
using System.Text;
using EasyPacketSharp.Abstract;

namespace EasyPacketSharp.Generic
{
    public class ImmutablePacket : IImmutablePacket
    {
        public ulong Length => (ulong)Reader.BaseStream.Length;
        public ulong Position => (ulong)Reader.BaseStream.Position;
        public bool Locked { get; private set; }

        private BinaryReader Reader { get; set; }
        private Encoding Encoding { get; }

        /// <summary>
        ///     Creates a readable packet by default and unless supplied assumes UTF8 Encoding
        /// </summary>
        /// <param name="buffer">The buffer to create the packet from</param>
        /// <param name="enc">An ecoding to read the stream with</param>
        public ImmutablePacket(byte[] buffer, Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;
            Reader = new BinaryReader(new MemoryStream(buffer), enc);
            Encoding = enc;
        }

        public void Focus(ulong at)
        {
            if (at > int.MaxValue) throw new NotImplementedException("This implementation does not support ulong lengths.");
            var buffer = new byte[Length];
            Buffer.BlockCopy(((MemoryStream)Reader.BaseStream).GetBuffer(), 0, buffer, 0, (int)at);
            Reader = new BinaryReader(new MemoryStream(buffer), Encoding);
        }

        public byte[] Lock()
        {
            if (Locked) throw new InvalidOperationException("The buffer mutation is already locked.");
            Locked = true;
            return ((MemoryStream) Reader.BaseStream).ToArray();
        }

        public void Unlock()
        {
            if (!Locked) throw new InvalidOperationException("The buffer mutation is already unlocked.");
            Locked = false;
        }

        public byte[] Read(uint length)
        {
            if (length > int.MaxValue) throw new NotImplementedException("This implementation does not support uint lengths.");
            return Reader.ReadBytes((int)length);
        }

        public int ReadInt()
        {
            return Reader.ReadInt32();
        }

        public uint ReadUInt()
        {
            return Reader.ReadUInt32();
        }

        public long ReadLong()
        {
            return Reader.ReadInt64();
        }

        public ulong ReadULong()
        {
            return Reader.ReadUInt64();
        }

        public short ReadShort()
        {
            return Reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return Reader.ReadUInt16();
        }

        public byte ReadByte()
        {
            return Reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return Reader.ReadSByte();
        }

        public float ReadFloat()
        {
            return Reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return Reader.ReadDouble();
        }

        public string ReadString(ulong length)
        {
            if (length > int.MaxValue) throw new NotImplementedException("This implementation does not support uint lengths.");
            return new string(Reader.ReadChars((int)length));
        }

        public string ReadString()
        {
            return Reader.ReadString();
        }

        public string ReadStandardString()
        {
            return new string(Reader.ReadChars(ReadUShort()));
        }
    }
}
