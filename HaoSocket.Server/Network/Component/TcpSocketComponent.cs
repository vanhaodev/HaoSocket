using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HaoSocket.Server.Network.Component
{
    public struct TcpSessionComponent
    {
        public Socket Socket;                // Socket TCP
        public EndPoint RemoteEndPoint;      // IP:Port
        public int SessionId;                // ID session nội bộ
        public bool IsDisconnected;          // Đánh dấu đã ngắt kết nối

        public void Init(Socket socket, int sessionId)
        {
            Socket = socket;
            RemoteEndPoint = socket.RemoteEndPoint!;
            SessionId = sessionId;
            IsDisconnected = false;
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    return !(IsDisconnected || (Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0));
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Disconnect()
        {
            if (IsDisconnected) return;
            IsDisconnected = true;
            try { Socket.Shutdown(SocketShutdown.Both); } catch { }
            try { Socket.Close(); } catch { }
        }
    }
}
