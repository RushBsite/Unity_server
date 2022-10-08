using System;
using System.Collections.Generic;
using Protocol;
using FlatBuffers;
using System.Text;
using Com.Amazon.Whitewater.Auxproxy.Pbuffer;
using Aws.GameLift.Server;
using System.Drawing.Drawing2D;
using Server.Game.Object;
using Player = Server.Game.Object.Player;

namespace Server.Game.Room
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        List<Player> _players = new List<Player>();

        private FlatBufferBuilder fbb = new FlatBufferBuilder(1024);

        public void EnterGame(Player newPlayer)
        {
            if (newPlayer == null)
                return;

            lock (_lock)
            {
                _players.Add(newPlayer);
                newPlayer.Room = this;

                //본인한테 전송
                {
                    fbb.Clear();
                    //내가 왔다고 알리세용
                    S_EnterGameT enterPacket = new S_EnterGameT();
                    enterPacket.Player = newPlayer.Info;
                    fbb.Finish(S_EnterGame.Pack(fbb, enterPacket).Value);
                    newPlayer.Session.Send(fbb.DataBuffer, (ushort)fbsId.S_EnterGame);

                    //나에게 상대 스폰을 알리세용
                    fbb.Clear();
                    S_SpawnT spawnPacket = new S_SpawnT();

                    foreach (Player p in _players)
                    {
                        if (newPlayer != p)
                        {
                            spawnPacket.Players = new List<PlayerInfoT>();
                            spawnPacket.Players.Add(p.Info);
                        }
                    }
                    fbb.Finish(S_Spawn.Pack(fbb, spawnPacket).Value);
                    newPlayer.Session.Send(fbb.DataBuffer, (ushort)fbsId.S_Spawn);
                }
                //타인한테 전송

                {
                    //나의 스폰을 알리세용
                    fbb.Clear();
                    S_SpawnT spawnPacket = new S_SpawnT();
                    spawnPacket.Players = new List<PlayerInfoT>();
                    spawnPacket.Players.Add(newPlayer.Info);
                    fbb.Finish(S_Spawn.Pack(fbb, spawnPacket).Value);
                    foreach (Player p in _players)
                    {
                        if (newPlayer != p)
                            p.Session.Send(fbb.DataBuffer, (ushort)fbsId.S_Spawn);
                    }

                }

                //Gamelift 갱신
                GameLiftServerAPI.AcceptPlayerSession(newPlayer.Session.SessionId.ToString());
            }
        }

        public void LeaveGame(int playerId)
        {
            lock (_lock)
            {
                Player player = _players.Find(p => p.Info.PlayerId == playerId);

                if (player == null)
                    return;

                _players.Remove(player);
                player.Room = null;

                //본인한테 정보 전송
                {
                    fbb.Clear();
                    S_LeaveGameT leavePacket = new S_LeaveGameT();
                    fbb.Finish(S_LeaveGame.Pack(fbb, leavePacket).Value);
                    player.Session.Send(fbb.DataBuffer, (ushort)fbsId.S_LeaveGame);

                }

                //타인한테 정보 전송

                {
                    //TODO : disconnect 시 처리?
                    fbb.Clear();
                    S_DespawnT despawnPacket = new S_DespawnT();
                    despawnPacket.PlayerIds = new List<int>();
                    despawnPacket.PlayerIds.Add(player.Info.PlayerId);
                    fbb.Finish(S_Despawn.Pack(fbb, despawnPacket).Value);
                    foreach (Player p in _players)
                    {
                        if (player != p)
                            p.Session.Send(fbb.DataBuffer, (ushort)fbsId.S_Despawn);
                    }
                }

                //Gamelift 갱신
                GameLiftServerAPI.RemovePlayerSession(player.Session.SessionId.ToString());

            }
        }

        public void HandleMove(Player player, C_MoveT movePacket)
        {


        }

        public void Update()
        {

        }

        public void Broadcast(ByteBuffer bb, ushort protocolId)
        {
            lock (_lock)
            {
                foreach (Player p in _players)
                {
                    p.Session.Send(bb, protocolId);
                }
            }
        }



    }
}
