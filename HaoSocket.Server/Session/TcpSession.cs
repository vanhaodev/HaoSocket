using System;
using System.Net;
using System.Net.Sockets;

namespace HaoSocket.Server.Session
{
    public class TcpSession
    {
        private readonly Socket _socket;
        private readonly byte[] _buffer;

        public EndPoint RemoteEndPoint => _socket.RemoteEndPoint!;
        public bool IsConnected => _socket.Connected;

        public byte[] Buffer => _buffer;
        public int Offset { get; set; } = 0;

        public Action<TcpSession, ArraySegment<byte>>? OnDataReceived;
        public Action<TcpSession>? OnDisconnected;

        public TcpSession(Socket socket, int bufferSize = 1024)
        {
            _socket = socket;
            _buffer = new byte[bufferSize];
        }

        public void ReceiveAvailable()
        {
            if (!_socket.Poll(0, SelectMode.SelectRead)) return;

            try
            {
                int received = _socket.Receive(_buffer);
                if (received <= 0)
                {
                    Disconnect();
                    return;
                }

                Offset = 0; // reset để decode từ đầu
                var segment = new ArraySegment<byte>(_buffer, 0, received);
                OnDataReceived?.Invoke(this, segment);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Send(ArraySegment<byte> data)
        {
            try
            {
                _socket.Send(data.Array!, data.Offset, data.Count, SocketFlags.None);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (_socket.Connected)
            {
                try { _socket.Shutdown(SocketShutdown.Both); } catch { }
                try { _socket.Close(); } catch { }
            }

            OnDisconnected?.Invoke(this);
        }
    }
}
