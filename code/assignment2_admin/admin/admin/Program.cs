using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;

namespace admin
{
    class Program
    {
        static void Main(string[] args)
        {
            //IPAddress localAddr = Dns.GetHostEntry("localhost").AddressList[0];
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            Console.WriteLine(localAddr);
            TcpListener server = new TcpListener(localAddr, 8000);
            Console.WriteLine("Waiting for a connection...");
            server.Start();
            Socket socket = server.AcceptSocket();
            Console.WriteLine("Connection accepted.");
            Console.WriteLine("tcpListner started");

            Admin newAdmin = new Admin();
            newAdmin.StartGame(socket);
            server.Stop();

            return;
        }
    }
}
