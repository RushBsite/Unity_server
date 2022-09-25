using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using FlatBuffers;
using System.IO;
using Server.Game;

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
            RoomManager.Instance.Add(); //1번룸만 일단 사용


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
