using System;
using System.Text;
using HaoSocket.Shared.Packet;
namespace HaoSocket.Shared.Demo
{
    public class ChatDemo
    {
        private readonly PacketHelper _packetHelper;

        public ChatDemo(PacketHelper packetHelper)
        {
            _packetHelper = packetHelper;
        }

        public void WriteChatPacket(byte[] buffer, ref int offset, string message)
        {
            _packetHelper.WriteInt(buffer, ref offset, 2001);           // MsgId
            _packetHelper.WriteString(buffer, ref offset, message);     // Nội dung
        }

        public string ReadChatMessage(byte[] buffer, ref int offset)
        {
            return _packetHelper.ReadString(buffer, ref offset);
        }
    }
}
