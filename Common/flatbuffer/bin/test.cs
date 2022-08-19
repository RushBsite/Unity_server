// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace PlayerSample
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public enum fbsId : byte
{
  NONE = 0,
  S_BroadcastEnterGame = 1,
  C_LeaveGame = 2,
  S_BroadcastLeaveGame = 3,
  S_PlayerList = 4,
  C_Move = 5,
  S_BroadcastMove = 6,
};

public struct Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<PlayerSample.Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<PlayerSample.Vec3>(builder.Offset);
  }
};

public struct Player : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Player __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public bool IsSelf { get { return 0!=__p.bb.Get(__p.bb_pos + 0); } }
  public int PlayerId { get { return __p.bb.GetInt(__p.bb_pos + 4); } }
  public PlayerSample.Vec3 Pos { get { return (new PlayerSample.Vec3()).__assign(__p.bb_pos + 8, __p.bb); } }

  public static Offset<PlayerSample.Player> CreatePlayer(FlatBufferBuilder builder, bool IsSelf, int PlayerId, float pos_X, float pos_Y, float pos_Z) {
    builder.Prep(4, 20);
    builder.Prep(4, 12);
    builder.PutFloat(pos_Z);
    builder.PutFloat(pos_Y);
    builder.PutFloat(pos_X);
    builder.PutInt(PlayerId);
    builder.Pad(3);
    builder.PutBool(IsSelf);
    return new Offset<PlayerSample.Player>(builder.Offset);
  }
};

public struct S_BroadcastEnterGame : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static S_BroadcastEnterGame GetRootAsS_BroadcastEnterGame(ByteBuffer _bb) { return GetRootAsS_BroadcastEnterGame(_bb, new S_BroadcastEnterGame()); }
  public static S_BroadcastEnterGame GetRootAsS_BroadcastEnterGame(ByteBuffer _bb, S_BroadcastEnterGame obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public S_BroadcastEnterGame __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int PlayerId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public PlayerSample.Vec3? Pos { get { int o = __p.__offset(6); return o != 0 ? (PlayerSample.Vec3?)(new PlayerSample.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartS_BroadcastEnterGame(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddPlayerId(FlatBufferBuilder builder, int playerId) { builder.AddInt(0, playerId, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<PlayerSample.Vec3> posOffset) { builder.AddStruct(1, posOffset.Value, 0); }
  public static Offset<PlayerSample.S_BroadcastEnterGame> EndS_BroadcastEnterGame(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.S_BroadcastEnterGame>(o);
  }
};

public struct C_LeaveGame : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static C_LeaveGame GetRootAsC_LeaveGame(ByteBuffer _bb) { return GetRootAsC_LeaveGame(_bb, new C_LeaveGame()); }
  public static C_LeaveGame GetRootAsC_LeaveGame(ByteBuffer _bb, C_LeaveGame obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public C_LeaveGame __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }


  public static void StartC_LeaveGame(FlatBufferBuilder builder) { builder.StartTable(0); }
  public static Offset<PlayerSample.C_LeaveGame> EndC_LeaveGame(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.C_LeaveGame>(o);
  }
};

public struct S_BroadcastLeaveGame : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static S_BroadcastLeaveGame GetRootAsS_BroadcastLeaveGame(ByteBuffer _bb) { return GetRootAsS_BroadcastLeaveGame(_bb, new S_BroadcastLeaveGame()); }
  public static S_BroadcastLeaveGame GetRootAsS_BroadcastLeaveGame(ByteBuffer _bb, S_BroadcastLeaveGame obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public S_BroadcastLeaveGame __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int PlayerId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<PlayerSample.S_BroadcastLeaveGame> CreateS_BroadcastLeaveGame(FlatBufferBuilder builder,
      int player_id = 0) {
    builder.StartTable(1);
    S_BroadcastLeaveGame.AddPlayerId(builder, player_id);
    return S_BroadcastLeaveGame.EndS_BroadcastLeaveGame(builder);
  }

  public static void StartS_BroadcastLeaveGame(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddPlayerId(FlatBufferBuilder builder, int playerId) { builder.AddInt(0, playerId, 0); }
  public static Offset<PlayerSample.S_BroadcastLeaveGame> EndS_BroadcastLeaveGame(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.S_BroadcastLeaveGame>(o);
  }
};

public struct S_PlayerList : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static S_PlayerList GetRootAsS_PlayerList(ByteBuffer _bb) { return GetRootAsS_PlayerList(_bb, new S_PlayerList()); }
  public static S_PlayerList GetRootAsS_PlayerList(ByteBuffer _bb, S_PlayerList obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public S_PlayerList __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public PlayerSample.Player? Players(int j) { int o = __p.__offset(4); return o != 0 ? (PlayerSample.Player?)(new PlayerSample.Player()).__assign(__p.__vector(o) + j * 20, __p.bb) : null; }
  public int PlayersLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<PlayerSample.S_PlayerList> CreateS_PlayerList(FlatBufferBuilder builder,
      VectorOffset playersOffset = default(VectorOffset)) {
    builder.StartTable(1);
    S_PlayerList.AddPlayers(builder, playersOffset);
    return S_PlayerList.EndS_PlayerList(builder);
  }

  public static void StartS_PlayerList(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddPlayers(FlatBufferBuilder builder, VectorOffset playersOffset) { builder.AddOffset(0, playersOffset.Value, 0); }
  public static void StartPlayersVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(20, numElems, 4); }
  public static Offset<PlayerSample.S_PlayerList> EndS_PlayerList(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.S_PlayerList>(o);
  }
};

public struct C_Move : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static C_Move GetRootAsC_Move(ByteBuffer _bb) { return GetRootAsC_Move(_bb, new C_Move()); }
  public static C_Move GetRootAsC_Move(ByteBuffer _bb, C_Move obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public C_Move __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public PlayerSample.Vec3? Pos { get { int o = __p.__offset(4); return o != 0 ? (PlayerSample.Vec3?)(new PlayerSample.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartC_Move(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddPos(FlatBufferBuilder builder, Offset<PlayerSample.Vec3> posOffset) { builder.AddStruct(0, posOffset.Value, 0); }
  public static Offset<PlayerSample.C_Move> EndC_Move(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.C_Move>(o);
  }
};

public struct S_BroadcastMove : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_2_0_0(); }
  public static S_BroadcastMove GetRootAsS_BroadcastMove(ByteBuffer _bb) { return GetRootAsS_BroadcastMove(_bb, new S_BroadcastMove()); }
  public static S_BroadcastMove GetRootAsS_BroadcastMove(ByteBuffer _bb, S_BroadcastMove obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public S_BroadcastMove __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int PlayerId { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public PlayerSample.Vec3? Pos { get { int o = __p.__offset(6); return o != 0 ? (PlayerSample.Vec3?)(new PlayerSample.Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartS_BroadcastMove(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddPlayerId(FlatBufferBuilder builder, int playerId) { builder.AddInt(0, playerId, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<PlayerSample.Vec3> posOffset) { builder.AddStruct(1, posOffset.Value, 0); }
  public static Offset<PlayerSample.S_BroadcastMove> EndS_BroadcastMove(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<PlayerSample.S_BroadcastMove>(o);
  }
};


}
