using FlatBuffers;
using ChatTest;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler //수동관리
{
	FlatBufferBuilder fbb = new FlatBufferBuilder(1);
	public static void C_ChatHandler(PacketSession session, IFlatbufferObject packet)
	{
		C_Chat chatPacket = C_Chat.GetRootAsC_Chat(packet.ByteBuffer);

		ClientSession serverSession = session as ClientSession;

		Console.WriteLine(chatPacket.Context);
	}

}

