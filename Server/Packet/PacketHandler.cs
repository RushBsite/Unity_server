using FlatBuffers;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler //수동관리
{
    public static void C_LeaveGameHandler(PacketSession session, IFlatbufferObject packet)
    {
        ClientSession clientSession = session as ClientSession;
        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;
        clientSession.Room.Push(
            () => room.Leave(clientSession));
    }

    public static void C_MoveHandler(PacketSession session, IFlatbufferObject packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

        GameRoom room = clientSession.Room;
        clientSession.Room.Push(
            () => room.Move(clientSession, movePacket));
    }

}

