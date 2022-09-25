using FlatBuffers;
using Server;
using ServerCore;
using System;
using Protocol;

class PacketHandler //수동관리
{
	FlatBufferBuilder fbb = new FlatBufferBuilder(1);
	public static void C_MoveHandler(PacketSession session, IFlatbufferObject packet)
	{
		C_Move movePacket = C_Move.GetRootAsC_Move(packet.ByteBuffer);

		ClientSession serverSession = session as ClientSession;
	}

}

