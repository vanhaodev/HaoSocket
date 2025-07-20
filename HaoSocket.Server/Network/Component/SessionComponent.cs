using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HaoSocket.Server.Network.Component
{
    public struct SessionComponent
    {
        public Socket Socket;         // Raw socket
        public EndPoint RemoteEndPoint;
        public byte[] Buffer;         // Dùng chung buffer
        public int Offset;            // Offset decode
        public bool MarkedForDisconnect; // Đánh dấu để Disconnect ở System khác
    }
}
