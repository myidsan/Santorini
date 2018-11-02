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
                BinaryWriter writer = new BinaryWriter(nwstream);
                writer.Write(userInput);
                Console.WriteLine("sending : " + userInput);

                //// reads back text
                //BinaryReader reader = new BinaryReader(nwstream);
                //Console.WriteLine("received: " + reader.ToString());
                //----
                byte[] bytesToRead = new byte[clientSocket.ReceiveBufferSize];
                int bytesRead = nwstream.Read(bytesToRead, 0, clientSocket.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));

                //int i;
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);    
                //// Loop to receive all the data sent by the server.
                //while ((i = nwstream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    Console.WriteLine("in nwstream read");
                //    // Translate data bytes to a ASCII string.
                //    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                //    Console.WriteLine("Received: {0}", data);
                //}
            }
            Console.WriteLine("client socket closing");
            clientSocket.GetStream().Close();
            clientSocket.Close();

        }
    }
}
