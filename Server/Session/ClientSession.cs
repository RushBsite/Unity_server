using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Aws.GameLift.Server;
using FlatBuffers;
using Protocol;
using Server.Game;
using Server.Game.Object;
using Server.Game.Room;
using ServerCore;
namespace Server
{


    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public Player MyPlayer { get; set; }

        private FlatBufferBuilder fbb = new FlatBufferBuilder(1);

        public void Send(ByteBuffer bb, ushort protocolId) //TODO : tableT format
        {
            ushort size = (ushort)(bb.Length - bb.Position);
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(bb.ToSizedArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");



            fbb.Clear();
            // PROTOCOL test
            MyPlayer = PlayerManager.Instance.Add();
            {
                string p_name = $"Player_{MyPlayer.Info.PlayerId}";
                Encoding.ASCII.GetBytes(p_name).CopyTo(MyPlayer.Info.Name, 0);
                MyPlayer.Info.PosInfo.State = UserState.Idle;
                MyPlayer.Info.PosInfo.Pos.X = 0;
                MyPlayer.Info.PosInfo.Pos.Y = 0;
                MyPlayer.Info.PosInfo.Pos.Z = 0;
                //MyPlayer.Info.PosInfo.Angle = {0,0,0};
                MyPlayer.Session = this;
            }
            RoomManager.Instance.Find(1).EnterGame(MyPlayer);
          
        }


        public override void OnDisConnected(EndPoint endPoint)
        {
            RoomManager.Instance.Find(1).LeaveGame(MyPlayer.Info.PlayerId);

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
