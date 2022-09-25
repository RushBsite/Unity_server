using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public class RecvBuffer
    {
        // 5바이트 무결 recv 시
        // [r][ ][ ][ ][ ][w][ ][ ][ ][ ]
        // 읽기 진행 [][][][][][rw][][][][]

        // 8 바이트 결손 있을시
        // [r][][][][][w][][][][]
        // 나머지 3바이트 대기
        // [r][][][][][][][][w][]
        // 진행
        // [][][][][][][][][rw][]

        // case2 기본 크기 2byte 경우
        // 4바이트 결손 있을시(3바이트만 옴)
        // [ ][ ] | [r][w] | [ ][ ] 이상황에서 더이상 r 불가
        // [r][w] | [ ][ ] | [ ][ ] 첫번째로 옮겨서 다음 요청 w 부터 recv 할수 있게

        ArraySegment<byte> _buffer;

        //커서처럼 진행
        int _readPos;
        int _writePos; //쓸때 사용하는 커서

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        //데이터 크기
        public int DataSize { get { return _writePos - _readPos; } }

        //버퍼에 남은 공간
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        //현재까지 받은 데이터의 유효 범위 데이타
        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }
        //버퍼 여유공간 데이타
        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            int dataSize = DataSize;
            
            if(dataSize == 0)
            {
                //데이터 모두 처리됨, 남은 데이터 없으면 복사하지않고 커서 위치만 리셋
                _readPos = _writePos = 0;
            }
            else
            {
                //남은게 있으면 시작 위치로 복사
                //[][][r][][w] to [r][][w][][]
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > DataSize)
                return false;

            _readPos += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize)
                return false;

            _writePos += numOfBytes;
            return true;
        }

    }
}
