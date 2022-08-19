using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlatBuffers;
using PlayerSample;
using ServerCore;
namespace Server
{
	

	class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }


        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //버퍼 만들기
            var builder = new FlatBufferBuilder(1);
            var player1Id = 3;
            var pos = Vec3.CreateVec3(builder, 1.0f, 2.0f, 3.0f);

            //serialize
            S_BroadcastEnterGame.StartS_BroadcastEnterGame(builder);
            S_BroadcastEnterGame.AddPos(builder, pos);
            S_BroadcastEnterGame.AddPlayerId(builder, player1Id);
            var beg = S_BroadcastEnterGame.EndS_BroadcastEnterGame(builder);
            builder.Finish(beg.Value);
            ushort size = (ushort)builder.DataBuffer.Length;
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            ushort protocolId = 1;
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(builder.SizedByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));

            Program.Room.Push(() => Program.Room.Enter(this));
        }

        public override void OnDisConnected(EndPoint endPoint)
        {

            SessionManager.instance.Remove(this);
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
