using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcpClient
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            const int PORT = 8000;
            const string SERVER_IP = "127.0.0.1"; //localhost

            TcpClient client = new TcpClient(SERVER_IP, PORT);
            NetworkStream nwstream = client.GetStream();

            string userInput = Console.ReadLine();
            byte[] byteToSend = ASCIIEncoding.ASCII.GetBytes(userInput);

            // send the text
            nwstream.Write(byteToSend, 0, byteToSend.Length);
            Console.WriteLine("sending : " + userInput);

            // reads back text
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwstream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            client.Close();


        }
    }
}
