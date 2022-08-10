using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServerCore;

namespace DummyClient
{
    public abstract class Packet
    {
        public ushort size;
        public ushort packetId;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> s);
    }

    class PlayerInfoReq : Packet
    {
        public long playerId; //8byte

        public PlayerInfoReq()
        {
            this.packetId = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> s)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += 2;

            long playerId = BitConverter.ToInt64(s.Array, s.Offset + count);
            count += 8;
           
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);

            ushort count = 0;
            bool success = true;

            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset + count, openSegment.Count - count), this.packetId);
            count += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset + count, openSegment.Count - count), this.playerId);
            count += 8;

            success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset, openSegment.Count), count); //뒤에 넣어주는 크기 유의

            if (success == false)
                return null;

            return SendBufferHelper.Close(count);
        }
    }
   
    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,
    }
    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            PlayerInfoReq packet = new PlayerInfoReq() { packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001};
            //보낸다
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);

                ushort count = 0;
                bool success = true;
                //byte[] size = BitConverter.GetBytes(packet.size); // 2
                //byte[] packetId = BitConverter.GetBytes(packet.packetId); // 2
                //byte[] playerId = BitConverter.GetBytes(packet.playerId); // 8

                ////자동화 필요 -> offset 설정위해 지금까지 사용한 byte 정보 필요
                //ushort count = 0;

                //Array.Copy(size, 0, openSegment.Array, openSegment.Offset + count, 2);
                //count += 2;
                //Array.Copy(packetId, 0, openSegment.Array, openSegment.Offset + count, 2);
                //count += 2;
                //Array.Copy(playerId, 0, openSegment.Array, openSegment.Offset + count, 8);
                //count += 8;

                // 위에꺼 줄인 버전
                //success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset, openSegment.Count), packet.size);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset + count, openSegment.Count - count), packet.packetId);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset + count, openSegment.Count - count), packet.playerId);
                count += 8;

                success &= BitConverter.TryWriteBytes(new Span<byte>(openSegment.Array, openSegment.Offset, openSegment.Count), count); //뒤에 넣어주는 크기 유의


                ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);
                if (success)
                    Send(sendBuff);

            }

        }

        public override void OnDisConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        //이동 패킷 ((3,2) 로 이동하고 싶다!)
        // 이동(func enum 15) + (3 2) ex) 15 3 2
        // 무결성도 필요함 ex) 15 3 이렇게만들어오면 실행x

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
