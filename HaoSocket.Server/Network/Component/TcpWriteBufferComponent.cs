using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoSocket.Server.Network.Component
{
    public struct TcpWriteBufferComponent
    {
        public byte[] Buffer;   // Buffer nhận dữ liệu
        public int Start;       // Dữ liệu chưa xử lý bắt đầu từ đây
        public int End;         // Dữ liệu đã nhận đến đây
    }
}
