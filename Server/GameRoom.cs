using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using PlayerSample;
using FlatBuffers;

namespace Server
{
    class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }
        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);

            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
            // N ^ 2
            //foreach (ClientSession s in _sessions)
            //s.Send(segment);
        }
        public void Enter(ClientSession session)
        {
            //플레이어 추가
            _sessions.Add(session);
            session.Room = this;
            //신입생에게 모든 플레이어 목록 전송
            //S_PlayerList players = new S_PlayerList();
            var builder = new FlatBufferBuilder(1);
            foreach (ClientSession s in _sessions)
            {
                //players.players.Add(new S_PlayerList.Player()
                //{
                //    isSelf = (s == session),
                //    playerId = s.SessionId,
                //    posX = s.PosX,
                //    posY = s.PosY,
                //    posZ = s.PosZ,
                //});

                S_PlayerList.StartPlayersVector(builder, 1);
                Player.CreatePlayer(builder, (s == session), s.SessionId,s.PosX, s.PosY, s.PosZ);
                var players = builder.EndVector();

                S_PlayerList.StartS_PlayerList(builder);
                S_PlayerList.AddPlayers(builder, players);
                var pl = S_PlayerList.EndS_PlayerList(builder);

                builder.Finish(pl.Value);
            }
            ushort size = (ushort)builder.DataBuffer.Length;
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            ushort protocolId = 4;
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(builder.SizedByteArray(), 0, sendBuffer, 4, size);

            session.Send(new ArraySegment<byte>(sendBuffer));

            //신입생 입장을 모두에게 알린다
            var builder2 = new FlatBufferBuilder(1);
            //S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            //enter.playerId = session.SessionId;
            //enter.posX = 0;
            //enter.posY = 0;
            //enter.posZ = 0;
            //Broadcast(enter.Write());
            var pos = Vec3.CreateVec3(builder2, 0, 0, 0);
            S_BroadcastEnterGame.StartS_BroadcastEnterGame(builder2);
            S_BroadcastEnterGame.AddPlayerId(builder2, session.SessionId);
            S_BroadcastEnterGame.AddPos(builder2, pos);
            var sbeg = S_BroadcastEnterGame.EndS_BroadcastEnterGame(builder2);
            builder2.Finish(sbeg.Value);

            ushort size2 = (ushort)builder2.DataBuffer.Length;
            byte[] sendBuffer2 = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            ushort protocolId2 = 1;
            Array.Copy(BitConverter.GetBytes(protocolId2), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(builder.SizedByteArray(), 0, sendBuffer, 4, size);
            Broadcast(sendBuffer2);
        }
        public void Leave(ClientSession session)
        {
            //플레이어 제거
            _sessions.Remove(session);

            // 모두에게 알린다.
            //S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            //leave.playerId = session.SessionId;
            //Broadcast(leave.Write());
            var builder = new FlatBufferBuilder(1);
            S_BroadcastLeaveGame.StartS_BroadcastLeaveGame(builder);
            S_BroadcastLeaveGame.AddPlayerId(builder, session.SessionId);
            var sbl = S_BroadcastLeaveGame.EndS_BroadcastLeaveGame(builder);

            builder.Finish(sbl.Value);
            ushort size = (ushort)builder.DataBuffer.Length;
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            ushort protocolId = 3;
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(builder.SizedByteArray(), 0, sendBuffer, 4, size);

            Broadcast(sendBuffer);

        }

        public void Move(ClientSession session, C_Move packet)
        {
            ////좌표 바꾼다
            session.PosX = packet.Pos.Value.X;
            session.PosY = packet.Pos.Value.Y;
            session.PosZ = packet.Pos.Value.Z;

            ////모두에게 알린다
            //S_BroadcastMove move = new S_BroadcastMove();
            var builder = new FlatBufferBuilder(1);
            //move.playerId = session.SessionId;
            //move.posX = session.PosX;
            //move.posY = session.PosY;
            //move.posZ = session.PosZ;

            var pos = Vec3.CreateVec3(builder, session.PosX, session.PosY, session.PosZ);

            S_BroadcastMove.StartS_BroadcastMove(builder);
            S_BroadcastMove.AddPlayerId(builder, session.SessionId);
            S_BroadcastMove.AddPos(builder, pos);
            var sbm = S_BroadcastMove.EndS_BroadcastMove(builder);
            builder.Finish(sbm.Value);

            ushort size = (ushort)builder.DataBuffer.Length;
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            ushort protocolId = 6;
            Array.Copy(BitConverter.GetBytes(protocolId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(builder.SizedByteArray(), 0, sendBuffer, 4, size);

            Broadcast(sendBuffer);


        }
    }
}