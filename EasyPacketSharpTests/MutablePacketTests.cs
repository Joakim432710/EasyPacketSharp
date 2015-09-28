using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyPacketSharp.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPacketSharpTests
{
    [TestClass]
    public class MutablePacketTests
    {
        [TestMethod]
        public async Task TestPacketWriteByteArray()
        {
            var packet = new MutablePacket();
            packet.Write(new byte[] { 5, 11, 27, 30 });
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= 4 && bytes[0] == 5 && bytes[1] == 11 && bytes[2] == 27 && bytes[3] == 30, "Failed to write byte array to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.Write(new byte[] { 99, 12 }));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 72;
                var newBytes = packet.Lock();
                if (newBytes[0] != 5)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }


        [TestMethod]
        public async Task TestPacketWriteInt()
        {
            const int size = sizeof(int);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteInt(227);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 227 || bytes[lastIndex] == 227), "Failed to write integer to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteInt(333935));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 33;
                var newBytes = packet.Lock();
                if (newBytes[0] != 227 && newBytes[lastIndex] != 227)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteUint()
        {
            const int size = sizeof(uint);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteInt(201);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 201 || bytes[lastIndex] == 201), "Failed to write unsigned integer to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteInt(3935));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 94;
                var newBytes = packet.Lock();
                if (newBytes[0] != 201 && newBytes[lastIndex] != 201)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteLong()
        {
            const int size = sizeof (long);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteLong(19L);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 19 || bytes[lastIndex] == 19), "Failed to write long integer to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteLong(-132053L));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 245;
                var newBytes = packet.Lock();
                if (newBytes[0] != 19 && newBytes[lastIndex] != 19)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteULong()
        {
            const int size = sizeof(ulong);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteLong(139L);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 139 || bytes[lastIndex] == 139), "Failed to write unsigned long integer to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteULong(10505L));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 145;
                var newBytes = packet.Lock();
                if (newBytes[0] != 139 && newBytes[lastIndex] != 139)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteShort()
        {
            const int size = sizeof(short);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteShort(37);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 37 || bytes[lastIndex] == 37), "Failed to write short to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteShort(6452));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 185;
                var newBytes = packet.Lock();
                if (newBytes[0] != 37 && newBytes[lastIndex] != 37)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteUShort()
        {
            const int size = sizeof(ushort);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteShort(98);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 98 || bytes[lastIndex] == 98), "Failed to write unsigned short to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteUShort(1054));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 127;
                var newBytes = packet.Lock();
                if (newBytes[0] != 98 && newBytes[lastIndex] != 98)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteByte()
        {
            const int size = sizeof(byte);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteByte(210);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 210 || bytes[lastIndex] == 210), "Failed to write byte to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteByte(76));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 162;
                var newBytes = packet.Lock();
                if (newBytes[0] != 210 && newBytes[lastIndex] != 210)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteSByte()
        {
            const int size = sizeof(sbyte);
            const int lastIndex = size - 1;

            var packet = new MutablePacket();
            packet.WriteSByte(120);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size && (bytes[0] == 120 || bytes[lastIndex] == 120), "Failed to write signed byte to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteSByte(-33));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 77;
                var newBytes = packet.Lock();
                if (newBytes[0] != 120 && newBytes[lastIndex] != 120)
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteFloat()
        {
            const int size = sizeof(float);

            var packet = new MutablePacket();
            packet.WriteFloat(2.0f);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size, "Failed to write float to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteFloat(-1930.2423f));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 11;
                var newBytes = packet.Lock();
                if (bytes[0] != newBytes[0])
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteDouble()
        {
            const int size = sizeof(double);

            var packet = new MutablePacket();
            packet.WriteDouble(55.0);
            var bytes = packet.Lock();
            Assert.IsTrue(bytes.Length >= size, "Failed to write double to packet");
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteDouble(2425.1246435));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }
            else
            {
                bytes[0] = 97;
                var newBytes = packet.Lock();
                if (bytes[0] != newBytes[0])
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteString()
        {
            const string str = "abcd";

            var packet = new MutablePacket();
            packet.WriteString(str);
            var bytes = packet.Lock();
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteString("blahblah"));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 238;
                var newBytes = packet.Lock();
                if (bytes[0] != newBytes[0])
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        [TestMethod]
        public async Task TestPacketWriteStandardString()
        {
            const string str = "kaka";

            var packet = new MutablePacket();

            packet.WriteStandardString(str);
            var bytes = packet.Lock();
            if (packet.Locked)
            {
                var task = new Task(() => packet.WriteString("sdgsgd"));
                task.Start();
                Assert.IsTrue(await TaskTimesOut(TimeSpan.FromSeconds(2), task), "Packet reported being locked after locking but was still mutable");
            }

            else
            {
                bytes[0] = 212;
                var newBytes = packet.Lock();
                if (bytes[0] != newBytes[0])
                    Assert.Fail("Packet reported not being locked but the buffer was returned by reference not copy. Return a full copy of the buffer if you wish to unlock else ensure you set the buffer to a locked state when Lock() is called.");
            }
        }

        private static async Task<bool> TaskTimesOut(TimeSpan t, Task task)
        {
            return await Task.WhenAny(task, Task.Delay(t)) != task;
        }
    }
}