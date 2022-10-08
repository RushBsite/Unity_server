using System;
using System.Net;
using ServerCore;
using Server.Game;
using Aws.GameLift.Server;
using System.Collections.Generic;
using Aws.GameLift;
using Server.GameLift;
using System.Threading;

namespace Server
{
    
    class Program
    {
        static AutoResetEvent terminatingEvent = new AutoResetEvent(false);
        static Listener _listener = new Listener();
        void OnApplicationQuit()
        {
            //Make sure to call GameLiftServerAPI.Destroy() when the application quits. This resets the local connection with GameLift's agent.
            GameLiftServerAPI.Destroy();
        }
        static void FlushRoom()
        {
            JobTimer.Instance.Push(FlushRoom, 250);
        }
        static void Main(string[] args)
        {
            var gamelift = new GameLiftNetwork();

            RoomManager.Instance.Add(); //1번룸만 일단 사용
            var listeningPort = 7777;

            //DNS (Domain Name System)
            // www.naver.com -> xxx.xxx.xx.xx
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); //domain to ip
            IPAddress ipAddr = ipHost.AddressList[0]; //ip list
            IPEndPoint endPoint = new IPEndPoint(ipAddr, listeningPort); //최종주소



            _listener.Init(endPoint, () => { return SessionManager.instance.Generate(); });
            
       
            Console.WriteLine("Listening...");
            gamelift.Init(listeningPort);
   
            //FlushRoom();
            JobTimer.Instance.Push(FlushRoom);

            while (true)
            {
                JobTimer.Instance.Flush();
            }

            terminatingEvent.WaitOne();
            gamelift.Ending();
        }

         



        
    }
}
