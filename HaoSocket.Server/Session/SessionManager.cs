using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace HaoSocket.Server.Session
{
    public class SessionManager
    {
        private Socket _listenerSocket;
        private readonly List<TcpSession> _sessions = new();
        private int _maxConnections = 256;

        // Callbacks sử dụng TcpSession trực tiếp
        public Action<TcpSession>? OnSessionConnected;
        public Action<TcpSession>? OnSessionDisconnected;
        public Action<TcpSession, ArraySegment<byte>>? OnDataReceived;


        public void Start(int port, int maxConnections = 256)
        {
            _maxConnections = maxConnections;

            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _listenerSocket.Listen(_maxConnections);

            Console.WriteLine($"[SessionManager] Listening on port {port}...");
        }

        public void Tick()
        {
            AcceptPendingClients();
            ReceiveFromAll();
            CleanupDisconnected();
        }

        private void AcceptPendingClients()
        {
            while (_listenerSocket.Poll(0, SelectMode.SelectRead))
            {
                var clientSocket = _listenerSocket.Accept();

                if (_sessions.Count >= _maxConnections)
                {
                    clientSocket.Close();
                    continue;
                }

                var session = new TcpSession(clientSocket);
                session.OnDataReceived = (s, len) =>
                {
                    OnDataReceived?.Invoke(s, s.Buffer);
                };
                session.OnDisconnected = (s) =>
                {
                    OnSessionDisconnected?.Invoke(s);
                };

                _sessions.Add(session);
                OnSessionConnected?.Invoke(session);
            }
        }

        private void ReceiveFromAll()
        {
            foreach (var session in _sessions)
            {
                session.ReceiveAvailable();
            }
        }

        private void CleanupDisconnected()
        {
            _sessions.RemoveAll(s => !s.IsConnected);
        }

        public void Stop()
        {
            foreach (var session in _sessions)
                session.Disconnect();

            _sessions.Clear();

            _listenerSocket.Close();
            Console.WriteLine("[SessionManager] Stopped.");
        }
    }
}
