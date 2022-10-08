using Protocol;
using FlatBuffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game.Object
{
    public class Player
    {

        public PlayerInfoT Info { get; set; } = new PlayerInfoT() { PosInfo = new PositionInfoT() };
        public Room.GameRoom Room { get; set; }
        public ClientSession Session { get; set; }


    }
}
