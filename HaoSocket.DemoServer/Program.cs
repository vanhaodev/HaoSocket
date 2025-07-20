using HaoSocket.Server.Session;
using HaoSocket.Shared.Demo;
using HaoSocket.Shared.Packet;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HaoSocket.DemoServer
{
    internal class Program
    {
        private static readonly byte[] _broadcastBuffer = new byte[512];

        static void Main()
        {
            var packetHelper = new PacketHelper();
            var chatDemo = new ChatDemo(packetHelper);
            var sessionManager = new SessionManager();
            var mySessions = new List<TcpSession>();

            sessionManager.OnSessionConnected = session =>
            {
                Console.WriteLine($"[+] Connected: {session.RemoteEndPoint}");
                mySessions.Add(session);
            };

            sessionManager.OnSessionDisconnected = session =>
            {
                Console.WriteLine($"[-] Disconnected: {session.RemoteEndPoint}");
                mySessions.Remove(session);
            };

            sessionManager.OnDataReceived = (session, segment) =>
            {
                int offset = segment.Offset;
                int msgId = packetHelper.ReadInt(segment.Array!, ref offset);

                if (msgId == 2001)
                {
                    string message = chatDemo.ReadChatMessage(segment.Array!, ref offset);
                    Console.WriteLine($"[Chat] {session.RemoteEndPoint}: {message}");

                    int sendOffset = 0;
                    chatDemo.WriteChatPacket(_broadcastBuffer, ref sendOffset, $"{session.RemoteEndPoint}: {message}");

                    var broadcastSegment = new ArraySegment<byte>(_broadcastBuffer, 0, sendOffset);
                    foreach (var s in mySessions)
                        if (s != session) s.Send(broadcastSegment);
                }
            };

            sessionManager.Start(7777);
            Console.WriteLine("=== Server listening on port 7777 ===");

            new Thread(() =>
            {
                while (true)
                {
                    var line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    Console.WriteLine($"[Chat] You (server): {line}");

                    int sendOffset = 0;
                    chatDemo.WriteChatPacket(_broadcastBuffer, ref sendOffset, $"Server: {line}");

                    var segment = new ArraySegment<byte>(_broadcastBuffer, 0, sendOffset);
                    foreach (var s in mySessions)
                        s.Send(segment);
                }
            }).Start();

            while (true)
            {
                sessionManager.Tick();
                Thread.Sleep(10);
            }
        }
    }
}
