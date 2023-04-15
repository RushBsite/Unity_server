using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    public class Listener
    {
        //문지기 (가들고있는 휴대폰)//AddressFamily => ipv4 or 6인지 . dns에서 자동 결정
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100) //초기화
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory; //비동기실행시 listener 호출 원함수로 되돌려줄 이벤트 위한 핸들러

            //문지기 교육 = 바인드
            _listenSocket.Bind(endPoint);

            //영업시작
            // backlog : 최대 대기수
            _listenSocket.Listen(backlog);

            for (int i=0; i<register; i++)
            {
                //pending 상태라면 서버로 해당정보 보내고 비동기처리한다. 이벤트 생성
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted); //소켓 async 이벤트 핸들러, 이벤트 완료되면 OnAcceptCompleted로
                RegisterAccept(args); //이벤트 등록후 호출
                                      //1. pending true 라면 RegisterAccpet -> pending == true임으로 조건문 안걸림 + Eventhandler args 콜백되면 -> OnAcceptCompleted로
                                      //2. pending false 라면 RegisterAccept -> 바로 OnAcceptCompleted로
            }
        }

        void RegisterAccept(SocketAsyncEventArgs args) //thread pool 에서 실행됨
        {
            args.AcceptSocket = null; //재사용 위해 초기화

            bool pending = _listenSocket.AcceptAsync(args); //무한대기(블로킹)가 아니라 비동기로 설정
            if (pending == false) //대기없이 바로 들어왔다면
                OnAcceptCompleted(null, args);//accept 완료
           
        }

        //redzone 이 밑으론 deadlock 유의
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success) //에러가없다
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            } 
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args); //다음 client위한호출
        }
    }
} 
