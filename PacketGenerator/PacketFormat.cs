﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    class PacketFormat
    {
        // {0}
        public static string managerFormat =
@"using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager //싱글톤구현
{{
    #region Singleton
    static PacketManager _instance;
    public static PacketManager Instance
    {{
        get
        {{
            if (_instance == null)
                _instance = new PacketManager();
            return _instance;
        }}
    }}
    #endregion

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>(); //delegate
        
    public void Register()
    {{
        {0}
    }}
    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {{
        //유요한 버퍼 넘어옴
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>> action = null;
        if (_onRecv.TryGetValue(id, out action))
            action.Invoke(session, buffer);
    }}

    void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new() //IPacket 에 new 가능해야만 (조건)
    {{
        T pkt = new T();
        pkt.Read(buffer); //역직렬화

        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(pkt.Protocol, out action))
            action.Invoke(session, pkt);

    }}
}}";
        //{0} 패킷 이름
        public static string managerRegisterFormat =
@"      _onRecv.Add((ushort)PacketID.{0}, MakePacket<{0}>);
        _handler.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);";

        //{0} 패킷/이름 번호 목록
        //{1} 패킷 목록
        public static string fileFormat =
@"using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerCore;

public enum PacketID
{{
    {0}
}}

interface IPacket
{{
	ushort Protocol {{ get; }}
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}}

{1}
";
        // {0} 패킷 이름
        // {1} 패킷 번호
        public static string packetEnumFormat =
@"{0} = {1},";

        // {0} 패킷 이름
        // {1} 맴버 변수들
        // {2} 맴버 변수 Read
        // {3} 맴버  변수 Write
        public static string packetFormat = //@붙으면 여러줄 문자열
@"
class {0} : IPacket
{{
    {1}

    public ushort Protocol {{ get {{ return (ushort)PacketID.{0}; }} }}

    public  void Read(ArraySegment<byte> segment)
    {{
        ushort count = 0;

        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset + count, segment.Count);
        count += sizeof(ushort);
        count += sizeof(ushort);
        {2}
        
    }}

    public  ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //범위 찝어줌

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),(ushort)PacketID.{0});// slice 하더라도 s 변화 x
        count += sizeof(ushort);
        {3}
        success &= BitConverter.TryWriteBytes(s, count); //뒤에 넣어주는 크기 유의
        if (success == false)
            return null;
             
        return SendBufferHelper.Close(count);
    }}
}}


";
           // {0} 변수 타입
           // {1} 변수 이름
        public static string memberFormat =
@"public {0} {1};";

        //{0} 리스트 이름 [대문자]
        //{1} 리스트 이름 [소문자]
        //{2} 맴버 변수들
        //{3} 맴버 변수 Read
        //{4} 맴버  변수 Write

        public static string memberListFormat =
@" public class {0}
{{
    {2}
    
    public void Read(ReadOnlySpan<byte> s, ref ushort count)
    {{
        {3}
    }}
    public bool Write(Span<byte> s, ref ushort count)
    {{
        bool success = true;
        {4}
        return success;
    }}

}}
public List<{0}> {1}s = new List<{0}>();";

        //{0} 변수명
        // {1} To변수타입
        // {2} 변수 타입
        public static string readFormat =
@"this.{0} = BitConverter.{1}(s.Slice(count, s.Length - count));
count += sizeof({2});
";
        // {0} 변수 이름
        // {1} 변수 형식
        public static string readByteFormat =
@"this.{0} = ({1})segment.Array[segment.Offset + count];
count += sizeof({1});";

        // {0} 변수 이름

        public static string readStringFormat =
@"
ushort {0}Len = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(s.Slice(count, {0}Len));
count += {0}Len;";
        
        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
for (int i = 0; i < {1}Len; i++)
{{
    {0} {1} = new {0}();
    {1}.Read(s, ref count);
    {1}s.Add({1});
}}";


        // {0} 변수 이름
        // {1} 변수 타입
        public static string writeFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count),this.{0});
count += sizeof({1});";

        public static string writeByteFormat =
@"segment.Array[segment.Offset + count] = (byte)this.{0};
count += sizeof({1});";

        // {0} 변수이름
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;";

        // {0} 리스트 이름 [대문자]
        // {1} 리스트 이름 [소문자]
        public static string writeListFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.{1}s.Count);
count += sizeof(ushort);
foreach({0} {1} in this.{1}s)
{{
    success &= {1}.Write(s, ref count);
}}";
    }
}
