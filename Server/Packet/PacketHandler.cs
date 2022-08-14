using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler //수동관리
{
    public static void C_ChatHandler(PacketSession session, IPacket packet)
    {
        C_Chat chatPacket = packet as C_Chat;
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        clientSession.Room.Push(
            () => clientSession.Room.Broadcast(clientSession, chatPacket.chat));
    }
   
}

