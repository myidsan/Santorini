using System;
using System.IO;
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

            TcpClient clientSocket = new TcpClient(SERVER_IP, PORT);

            string userInput;
            while((userInput = Console.ReadLine()) != null)
            {

                NetworkStream nwstream = clientSocket.GetStream();
                byte[] userInputAsyBytes = Encoding.ASCII.GetBytes(userInput);
                nwstream.Write(userInputAsyBytes, 0, userInputAsyBytes.Length);
                //BinaryWriter writer = new BinaryWriter(nwstream);
                //writer.Write(userInput);
                Console.WriteLine("sending : " + userInput);

                // reads back bytes 
                byte[] bytesToRead = new byte[clientSocket.ReceiveBufferSize];
                int bytesRead = nwstream.Read(bytesToRead, 0, clientSocket.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));

            }
            Console.WriteLine("client socket closing");
            clientSocket.GetStream().Close();
            clientSocket.Close();

        }
    }
}
