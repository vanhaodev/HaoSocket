using HaoSocket.Client.Session;
using HaoSocket.Shared.Demo;
using HaoSocket.Shared.Packet;
using System;
using System.Threading;

namespace HaoSocket.DemoClient
{
    internal class Program
    {
        private static readonly byte[] _sendBuffer = new byte[512];

        static void Main()
        {
            var packetHelper = new PacketHelper();
            var chatDemo = new ChatDemo(packetHelper);
            var session = new TcpSession("127.0.0.1", 7777);

            session.OnDataReceived = (s, len) =>
            {
                int offset = 0;
                int msgId = packetHelper.ReadInt(s.Buffer, ref offset);

                if (msgId == 2001)
                {
                    string message = chatDemo.ReadChatMessage(s.Buffer, ref offset);
                    Console.WriteLine($"[Chat] {s.RemoteEndPoint}: {message}");
                }
            };

            Console.WriteLine("=== Connected. Type message and press Enter ===");

            // Thread nhập chat
            new Thread(() =>
            {
                while (session.IsConnected)
                {
                    var line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int sendOffset = 0;
                    chatDemo.WriteChatPacket(_sendBuffer, ref sendOffset, line);
                    session.Send(new ArraySegment<byte>(_sendBuffer, 0, sendOffset));

                    Console.WriteLine($"[Chat] You: {line}");
                }
            }).Start();

            // Tick đọc
            while (session.IsConnected)
            {
                session.ReceiveAvailable();
                Thread.Sleep(10);
            }
        }
    }
}
