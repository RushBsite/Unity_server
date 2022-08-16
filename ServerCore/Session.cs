using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FlatBuffers;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;
        //[size(2)][packetId(2)][....]
 
        public sealed override int OnRecv(ArraySegment<byte> buffer)//다른 클래스가 오버라이드 못함
        {
            int processLen = 0;//진행도
            int packetCount = 0;

            while (true)
            {
                //헤더 파싱 가능 확인
                if (buffer.Count < HeaderSize)
                    break;
                //패킷 완전체 도착 확인 => c# 플랫버퍼 기능 못씀
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset); //맨앞 사이즈 추출
                if (buffer.Count < dataSize)
                    break;
                //Arraysegment는 스택에 생성됨. 공유메모리 아님(struct라)
                //여기까지 오면 패킷 조립 가능
                OnRecvPacket(new ArraySegment<byte>(buffer.Array,buffer.Offset,dataSize));
                packetCount++;

                processLen += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize); //다음부분
            }

            if(packetCount> 1)
                Console.WriteLine($"패킷 모아 보내기 : {packetCount}");
            
            return processLen;
        }
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }

    //Gamesession 위해 추상 클래스화
    public abstract class Session
    {
        Socket _socket;
        int _disconnected = 0;

        //session 과 recvbuffer 1:1
        RecvBuffer _recvBuffer = new RecvBuffer(65535);

        object _lock = new object();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        //client recv session 이벤트 핸들러 만드는 2가지 방법
        //1) 여기에 이벤트(세션) 핸들러 등록 (public 함수들 제작)
        //2) 서버에서 session 상속한 Gamesession : session 인터페이스 만들어서 제작

        public abstract void OnConnected(EndPoint endPoint);
        public abstract int OnRecv(ArraySegment<byte> buffer); //데이터 처리량 리턴
        public abstract void OnSend(int numOfBytes);
        public abstract void OnDisConnected(EndPoint endPoint);

        void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _pendingList.Clear();
            }
        }


        public void Start(Socket socket)
        {
            _socket = socket;
             
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv(recvArgs);
        }
        //보낸다
        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock) //동시다발적으로 sendbuff 접근해서 무분별하게 보내면 오류 -> lock
            {
                _sendQueue.Enqueue(sendBuff); //바로 레지스터에 보내는게 아니라 일단 큐에 넣는다.
                if (_pendingList.Count == 0)
                    RegisterSend();
            }

        }
        public void Send(List<ArraySegment<byte>> sendBuffList)
        {
            if (sendBuffList.Count == 0)
                return;
            lock (_lock) //동시다발적으로 sendbuff 접근해서 무분별하게 보내면 오류 -> lock
            {
                foreach (ArraySegment<byte> sendBuff in sendBuffList)
                {
                    _sendQueue.Enqueue(sendBuff);
                }
                if (_pendingList.Count == 0)
                    RegisterSend();
            }

        }
        //쫓아낸다. 중복 disconnect 막기 위해 Interlocked Exchange 도입
        public void Disconnect()
        {


            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            OnDisConnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();
        }

        //받는다
        #region 네트워크 통신

        void RegisterSend()
        {
            if (_disconnected == 1)
                return;
    
            //send의 경우 서버가 어느시점에 전송할지 모르기때문에 예약 불가능
            //
            // C#은 포인터 없음으로(주소이동) offset 사용

            //_pendingList.Clear();
            while (_sendQueue.Count>0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);
            }

            _sendArgs.BufferList = _pendingList;

            try
            {
                bool pending = _socket.SendAsync(_sendArgs);
                if (pending == false)
                    OnSendCompleted(null, _sendArgs);
            }
            catch (Exception e)
            {

                Console.WriteLine($"RegisterSend Failed {e}");
            }

            
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args) 
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _pendingList.Clear();

                        OnSend(_sendArgs.BytesTransferred);
                        

                        if(_sendQueue.Count >0) //큐에 남은게 있다면 다시 레지스터로
                            RegisterSend();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }

        }
        void RegisterRecv(SocketAsyncEventArgs args)
        {
            if (_disconnected == 1)
                return;
            _recvBuffer.Clean();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            args.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(args);
                if (pending == false)
                    OnRecvCompleted(null, args);
            }
            catch (Exception e )
            {

                Console.WriteLine($"RegisterRecv Failed {e}");
            }

           
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                
                try
                {
                    //Write 커서 이동
                    if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    //컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지를 받는다.
                    int processLen = OnRecv(_recvBuffer.ReadSegment);
                    if(processLen < 0 || _recvBuffer.DataSize < processLen)
                    {
                        Disconnect();
                        return;
                    }

                    //Read 커서 이동
                    if(_recvBuffer.OnRead(processLen)==false)
                    {
                        Disconnect();
                        return;
                    }

                    RegisterRecv(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
                
            }
            else
            {
                Disconnect();
            }

        }

        #endregion
    }
}
