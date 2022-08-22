using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlatBuffers;
using ChatTest;
using ServerCore;
namespace Server
{
	

	class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        private FlatBufferBuilder fbb = new FlatBufferBuilder(1);

        public void Send(ByteBuffer bb, ushort protocolId)
        {           
            ushort size = (ushort)(bb.Length - bb.Position);
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(bb.ToSizedArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            fbb.Clear();
            // FB test
            var context = fbb.CreateString("안녕하세요");
            S_Chat.StartS_Chat(fbb);
            S_Chat.AddContext(fbb, context);
            var chatPacket = S_Chat.EndS_Chat(fbb);
            fbb.Finish(chatPacket.Value);

            Send(fbb.DataBuffer, (ushort)fbsId.S_Chat);
          
        }

        public override void OnDisConnected(EndPoint endPoint)
        {

            SessionManager.instance.Remove(this);
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            //PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
