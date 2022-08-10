using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServerCore;

namespace DummyClient
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //DNS (Domain Name System)
            // www.naver.com -> xxx.xxx.xx.xx
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host); //domain to ip
            IPAddress ipAddr = ipHost.AddressList[0]; //ip list
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //최종주소

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });

            while (true)
            {
                try
                {
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }


            
           

        }
    }
}
