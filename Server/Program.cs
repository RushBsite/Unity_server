﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    
    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            //DNS (Domain Name System)
            // www.naver.com -> xxx.xxx.xx.xx
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); //domain to ip
            IPAddress ipAddr = ipHost.AddressList[0]; //ip list
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //최종주소



            _listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening...");

            while (true)
            {
                //main thread loop
            }



        }
    }
}