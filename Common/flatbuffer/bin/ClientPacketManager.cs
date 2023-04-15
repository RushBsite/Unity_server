using FlatBuffers;
using Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IFlatbufferObject>> _handler = new Dictionary<ushort, Action<PacketSession, IFlatbufferObject>>();
		
	public void Register()
	{		
		_onRecv.Add((ushort)fbsId.S_EnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)fbsId.S_EnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)fbsId.S_LeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)fbsId.S_LeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)fbsId.S_Spawn, MakePacket<S_Spawn>);
		_handler.Add((ushort)fbsId.S_Spawn, PacketHandler.S_SpawnHandler);		
		_onRecv.Add((ushort)fbsId.S_Despawn, MakePacket<S_Despawn>);
		_handler.Add((ushort)fbsId.S_Despawn, PacketHandler.S_DespawnHandler);		
		_onRecv.Add((ushort)fbsId.S_Move, MakePacket<S_Move>);
		_handler.Add((ushort)fbsId.S_Move, PacketHandler.S_MoveHandler);		
		_onRecv.Add((ushort)fbsId.S_LoadMap, MakePacket<S_LoadMap>);
		_handler.Add((ushort)fbsId.S_LoadMap, PacketHandler.S_LoadMapHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IFlatbufferObject, new()
	{
		T pkt = new T();
		byte[] recvBuffer = new byte[buffer.Count - 4];
		Array.Copy(buffer.Array, buffer.Offset + 4, recvBuffer, 0, buffer.Count - 4);
		ByteBuffer bb = new ByteBuffer(recvBuffer);
		pkt.__init(id, bb);

		Action<PacketSession, IFlatbufferObject> action = null;
		if (_handler.TryGetValue(id, out action))
			action.Invoke(session, pkt);
	}

	public Action<PacketSession, IFlatbufferObject> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IFlatbufferObject> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}