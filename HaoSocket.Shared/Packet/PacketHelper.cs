using System;
using System.Collections.Generic;
using System.Text;

namespace HaoSocket.Shared.Packet
{
    public partial class PacketHelper
    {
        // ==
        public void WriteBool(byte[] buffer, ref int offset, bool value)
        {
            buffer[offset++] = (byte)(value ? 1 : 0);
        }

        public bool ReadBool(byte[] buffer, ref int offset)
        {
            return buffer[offset++] == 1;
        }
        // ==

        public void WriteByte(byte[] buffer, ref int offset, byte value)
        {
            buffer[offset] = value;
            offset += 1;
        }
        public byte ReadByte(byte[] buffer, ref int offset)
        {
            byte value = buffer[offset];
            offset += 1;
            return value;
        }
        // ==
        public void WriteSByte(byte[] buffer, ref int offset, sbyte value)
        {
            buffer[offset] = (byte)value;
            offset += 1;
        }

        public sbyte ReadSByte(byte[] buffer, ref int offset)
        {
            sbyte value = (sbyte)buffer[offset];
            offset += 1;
            return value;
        }
        // ==
        public void WriteShort(byte[] buffer, ref int offset, short value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
            offset += 2;
        }

        public short ReadShort(byte[] buffer, ref int offset)
        {
            short value = BitConverter.ToInt16(buffer, offset);
            offset += 2;
            return value;
        }
        // ==
        public void WriteUShort(byte[] buffer, ref int offset, ushort value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
            offset += 2;
        }

        public ushort ReadUShort(byte[] buffer, ref int offset)
        {
            ushort value = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
            return value;
        }
        // ==
        public void WriteInt(byte[] buffer, ref int offset, int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public int ReadInt(byte[] buffer, ref int offset)
        {
            int value = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            return value;
        }
        // ==
        public void WriteUInt(byte[] buffer, ref int offset, uint value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }
        public uint ReadUInt(byte[] buffer, ref int offset)
        {
            uint value = BitConverter.ToUInt32(buffer, offset);
            offset += 4;
            return value;
        }

        // ==
        public void WriteFloat(byte[] buffer, ref int offset, float value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public float ReadFloat(byte[] buffer, ref int offset)
        {
            float value = BitConverter.ToSingle(buffer, offset);
            offset += 4;
            return value;
        }
        // ==
        public void WriteLong(byte[] buffer, ref int offset, long value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
            offset += 8;
        }

        public long ReadLong(byte[] buffer, ref int offset)
        {
            long value = BitConverter.ToInt64(buffer, offset);
            offset += 8;
            return value;
        }
        // ==
        public void WriteULong(byte[] buffer, ref int offset, ulong value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
            offset += 8;
        }

        public ulong ReadULong(byte[] buffer, ref int offset)
        {
            ulong value = BitConverter.ToUInt64(buffer, offset);
            offset += 8;
            return value;
        }

        // ==
        public void WriteString(byte[] buffer, ref int offset, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt(buffer, ref offset, bytes.Length);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
            offset += bytes.Length;
        }

        public string ReadString(byte[] buffer, ref int offset)
        {
            int len = ReadInt(buffer, ref offset);
            string result = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;
            return result;
        }
        // ==
        public void WriteDateTime(byte[] buffer, ref int offset, DateTime value)
        {
            WriteLong(buffer, ref offset, value.Ticks);
        }

        public DateTime ReadDateTime(byte[] buffer, ref int offset)
        {
            return new DateTime(ReadLong(buffer, ref offset));
        }
    }
}
