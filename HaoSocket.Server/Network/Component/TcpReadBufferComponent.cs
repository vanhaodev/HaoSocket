using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoSocket.Server.Network.Component
{
    public struct TcpReadBufferComponent
    {
        public byte[] Buffer;   // Buffer nhận dữ liệu
        public int Start;       // Dữ liệu chưa xử lý bắt đầu từ đây
        public int End;         // Dữ liệu đã nhận đến đây
        // Số byte dữ liệu đang còn trong buffer (chưa xử lý hoặc chưa gửi)
        public int Length => End - Start;

        // Số byte còn trống để ghi thêm vào buffer
        public int FreeSpace => Buffer.Length - End;

        // Đặt lại buffer về trạng thái ban đầu
        public void Clear() => Start = End = 0;

        // Trả về vùng bộ nhớ còn trống có thể ghi vào (ghi packet mới, nhận thêm từ socket)
        // => thường dùng với Socket.Receive() hoặc WritePacket()
        public ArraySegment<byte> GetWritableSegment() => new(Buffer, End, FreeSpace);

        // Trả về vùng dữ liệu đã có, đang chờ xử lý (gửi hoặc parse packet)
        // => thường dùng với Socket.Send() hoặc PacketParser
        public ArraySegment<byte> GetReadableSegment() => new(Buffer, Start, Length);

    }
}
