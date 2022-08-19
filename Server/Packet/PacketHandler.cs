using FlatBuffers;
using PlayerSample;
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
        var movePacket = C_Move.GetRootAsC_Move(packet.ByteBuffer);
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
            return;

        Console.WriteLine($"{movePacket.Pos.Value.X}, {movePacket.Pos.Value.Y}, {movePacket.Pos.Value.Z}");

        GameRoom room = clientSession.Room;
        clientSession.Room.Push(
            () => room.Move(clientSession, movePacket));
    }

}

