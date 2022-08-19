using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using FlatBuffers;
using PlayerSample;
using System.IO;

namespace Server
{
    
    class Program
    {
        
        static Listener _listener = new Listener();

        static void FlushRoom()
        {
            JobTimer.Instance.Push(FlushRoom, 250);
        }
        static void Main(string[] args)
        {
            //버퍼 만들기
            var builder = new FlatBufferBuilder(1);
            var player1Id = 3;
            var pos = Vec3.CreateVec3(builder, 1.0f, 2.0f, 3.0f);

            //serialize
            S_BroadcastEnterGame.StartS_BroadcastEnterGame(builder);
            S_BroadcastEnterGame.AddPos(builder, pos);
            S_BroadcastEnterGame.AddPlayerId(builder, player1Id);
            var beg = S_BroadcastEnterGame.EndS_BroadcastEnterGame(builder);

            builder.Finish(beg.Value);

            // 버퍼 풀기
            var buf = builder.DataBuffer;
            //root access
            var Beg = S_BroadcastEnterGame.GetRootAsS_BroadcastEnterGame(buf);
            var player1Idc = Beg.PlayerId;
            var vec = Beg.Pos.Value;
            Console.WriteLine($"{player1Idc}, vecX:{vec.X},vecY:{vec.Y},vecZ:{vec.Z}");


            
            //DNS (Domain Name System)
            // www.naver.com -> xxx.xxx.xx.xx
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); //domain to ip
            IPAddress ipAddr = ipHost.AddressList[0]; //ip list
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //최종주소



            _listener.Init(endPoint, () => { return SessionManager.instance.Generate(); });
            Console.WriteLine("Listening...");

            //FlushRoom();
            JobTimer.Instance.Push(FlushRoom);
            
            while (true)
            {
                JobTimer.Instance.Flush();
            }



        }
    }
}
