using FlatBuffers;
using Server;
using ServerCore;
using System;
using Protocol;

class PacketHandler //수동관리
{
	private static FlatBufferBuilder fb = new FlatBufferBuilder(1024);
	public static void C_MoveHandler(PacketSession session, IFlatbufferObject packet)
	{
		C_Move movePacket = C_Move.GetRootAsC_Move(packet.ByteBuffer);

		ClientSession clientSession = session as ClientSession;

		Console.WriteLine($"X:{movePacket.PosInfo.Value.Pos.X},Y:{movePacket.PosInfo.Value.Pos.Y},Z:{movePacket.PosInfo.Value.Pos.Z}");

		//broadcast

		if (clientSession.MyPlayer == null)
			return;
		if (clientSession.MyPlayer.Room == null)
			return;

		//TODO : 검증

		//일단 서버에서 좌표 위치 이동 계산
		PlayerInfoT info = clientSession.MyPlayer.Info;
		info.PosInfo = movePacket.UnPack().PosInfo;


		//다른 플레이어에게 알린다.
		S_MoveT resMovePacket = new S_MoveT();
		resMovePacket.PlayerId = clientSession.MyPlayer.Info.PlayerId;
		resMovePacket.PosInfo = info.PosInfo;


		fb.Clear();
		fb.Finish(S_Move.Pack(fb, resMovePacket).Value);
		clientSession.MyPlayer.Room.Broadcast(fb.DataBuffer, (ushort)fbsId.S_Move);
	}

}

