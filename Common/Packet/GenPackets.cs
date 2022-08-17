using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerCore;

public enum PacketID
{
    S_BroadcastEnterGame = 1,
	C_LeaveGame = 2,
	S_BroadcastLeaveGame = 3,
	S_PlayerList = 4,
	C_Move = 5,
	S_BroadcastMove = 6,
	
}

public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}


class S_BroadcastEnterGame : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.playerId = BitConverter.ToInt32(s.Slice(count, s.Length - count));
		count += sizeof(int);
		
		this.posX = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.S_BroadcastEnterGame);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.playerId);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posZ);
		count += sizeof(float);
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



class C_LeaveGame : IPacket
{
    

    public ushort Protocol { get { return (ushort)PacketID.C_LeaveGame; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.C_LeaveGame);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



class S_BroadcastLeaveGame : IPacket
{
    public int playerId;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastLeaveGame; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.playerId = BitConverter.ToInt32(s.Slice(count, s.Length - count));
		count += sizeof(int);
		
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.S_BroadcastLeaveGame);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.playerId);
		count += sizeof(int);
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



class S_PlayerList : IPacket
{
     public class Player
	{
	    public bool isSelf;
		public int playerId;
		public float posX;
		public float posY;
		public float posZ;
	    
	    public void Read(ReadOnlySpan<byte> s, ref ushort count)
	    {
	        this.isSelf = BitConverter.ToBoolean(s.Slice(count, s.Length - count));
			count += sizeof(bool);
			
			this.playerId = BitConverter.ToInt32(s.Slice(count, s.Length - count));
			count += sizeof(int);
			
			this.posX = BitConverter.ToSingle(s.Slice(count, s.Length - count));
			count += sizeof(float);
			
			this.posY = BitConverter.ToSingle(s.Slice(count, s.Length - count));
			count += sizeof(float);
			
			this.posZ = BitConverter.ToSingle(s.Slice(count, s.Length - count));
			count += sizeof(float);
			
	    }
	    public bool Write(Span<byte> s, ref ushort count)
	    {
	        bool success = true;
	        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.isSelf);
			count += sizeof(bool);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.playerId);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posX);
			count += sizeof(float);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posY);
			count += sizeof(float);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posZ);
			count += sizeof(float);
	        return success;
	    }
	
	}
	public List<Player> players = new List<Player>();

    public ushort Protocol { get { return (ushort)PacketID.S_PlayerList; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		for (int i = 0; i < playerLen; i++)
		{
		    Player player = new Player();
		    player.Read(s, ref count);
		    players.Add(player);
		}
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.S_PlayerList);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.players.Count);
		count += sizeof(ushort);
		foreach(Player player in this.players)
		{
		    success &= player.Write(s, ref count);
		}
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



class C_Move : IPacket
{
    public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.C_Move; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.posX = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.C_Move);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posZ);
		count += sizeof(float);
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



class S_BroadcastMove : IPacket
{
    public int playerId;
	public float posX;
	public float posY;
	public float posZ;

    public ushort Protocol { get { return (ushort)PacketID.S_BroadcastMove; } }

    public  void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        this.playerId = BitConverter.ToInt32(s.Slice(count, s.Length - count));
		count += sizeof(int);
		
		this.posX = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posY = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
		this.posZ = BitConverter.ToSingle(s.Slice(count, s.Length - count));
		count += sizeof(float);
		
        
    }

    public  ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.S_BroadcastMove);// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.playerId);
		count += sizeof(int);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posX);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posY);
		count += sizeof(float);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.posZ);
		count += sizeof(float);
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }
}



