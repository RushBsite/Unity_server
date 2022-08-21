using FlatBuffers;
using PlayerSample;
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
		_onRecv.Add((ushort)fbsId.S_Chat, MakePacket<S_Chat>);
		_handler.Add((ushort)fbsId.S_Chat, PacketHandler.S_ChatHandler);		
		_onRecv.Add((ushort)fbsId.S_EnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)fbsId.S_EnterGame, PacketHandler.S_EnterGameHandler);
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
		pkt.ByteBuffer.Put(buffer.Offset, buffer.Array);
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