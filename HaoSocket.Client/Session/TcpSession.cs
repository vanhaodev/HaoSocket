using System;
using System.Net;
using System.Net.Sockets;

namespace HaoSocket.Client.Session
{
    public class TcpSession
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly byte[] _buffer;

        public byte[] Buffer => _buffer;
        public int Offset { get; set; }

        public EndPoint RemoteEndPoint =>
            _client.Client?.RemoteEndPoint ?? new IPEndPoint(IPAddress.None, 0);

        public bool IsConnected => _client.Connected;

        public Action<TcpSession, ArraySegment<byte>> OnDataReceived;
        public Action<TcpSession> OnDisconnected;

        public TcpSession(string host, int port, int bufferSize = 1024)
        {
            _client = new TcpClient();
            _client.Connect(host, port);
            _stream = _client.GetStream();
            _buffer = new byte[bufferSize];
            Offset = 0;
        }

        public void ReceiveAvailable()
        {
            try
            {
                if (_client.Available <= 0) return;

                int received = _stream.Read(_buffer, 0, _buffer.Length);
                if (received <= 0)
                {
                    Disconnect();
                    return;
                }

                Offset = 0;
                var segment = new ArraySegment<byte>(_buffer, 0, received);
                OnDataReceived?.Invoke(this, segment);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Send(ArraySegment<byte> segment)
        {
            try
            {
                if (segment.Array != null)
                {
                    _stream.Write(segment.Array, segment.Offset, segment.Count);
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try { _stream.Close(); } catch { }
            try { _client.Close(); } catch { }

            OnDisconnected?.Invoke(this);
        }
    }
}
