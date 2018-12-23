using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Program
    {
        /// <summary>
        ///  Test harness for the board -- assign3
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        //public static void Main(string[] args)
        //{
        //    JSONEncoder parser = new JSONEncoder();
        //    JToken input;

        //    // dispatch
        //    while (true)
        //    {
        //        input = parser.JSONParser();
        //        if (input == null) break;
        //        Board newBoard = new Board((JArray)input[0]);
        //        //newBoard.PrintBoard();

        //        JArray action = (JArray)input[1];
        //        newBoard.RunCommand(action);
        //    }
        //    return;
        //}

        /// <summary>
        /// Test harness for the rule checker -- assign4
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        //public static void Main(string[] args)
        //{
        //    JSONEncoder parser = new JSONEncoder();
        //    //RuleChecker checker = new RuleChecker();
        //    JToken input;

        //    // dispatch -- reading from STDIN
        //    while (true)
        //    {
        //        input = parser.JSONParser();
        //        if (input == null) break;
        //        // validate board
        //        Board newBoard = new Board((JArray)input[0]);

        //        Board tempBoardObj;
        //        Cell[,] tempBoard;

        //        string JSONresult;

        //        if (!RuleChecker.IsBoardValid(newBoard))
        //        {
        //            JSONresult = JsonConvert.SerializeObject("no");
        //            Console.WriteLine(JSONresult);
        //            continue;
        //        }

        //        // validate move
        //        string worker = (string)input[1];
        //        JArray directions = (JArray)input[2];
        //        string[] stringDirections = directions.ToObject<string[]>();

        //        if (stringDirections.Length == 1)
        //        {
        //            if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
        //            {
        //                JSONEncoder.PrintJson("no");
        //                continue;
        //            }
        //            newBoard.Move(worker, stringDirections[0]);
        //            if (!RuleChecker.IsPlayerWinner(newBoard, worker))
        //            {
        //                JSONEncoder.PrintJson("no");
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
        //            {
        //                JSONEncoder.PrintJson("no");
        //                continue;
        //            }
        //            tempBoard = newBoard.Move(worker, stringDirections[0]);
        //            JArray test = new JArray();
        //            test = newBoard.DumpBoard();
        //            tempBoardObj = new Board(test);
        //            if (!RuleChecker.IsValidBuild(tempBoardObj, worker, stringDirections))
        //            {
        //                JSONEncoder.PrintJson("no");
        //                continue;
        //            }
        //        }
        //        JSONEncoder.PrintJson("yes");
        //    }
        //}

        /// <Summary>
        /// Test harness for the rule checker -- assign5, assign6
        /// </Summary>
        //public static void Main(string[] args)
        //{
        //    JSONEncoder parser = new JSONEncoder();

        //    JArray input = (JArray)parser.JSONParser();
        //    string color = input[1].ToString();
        //    Player newPlayer = new Player(color);
        //    newPlayer.RunCommand(input);

        //    // dispatch
        //    while (true)
        //    {
        //        input = (JArray)parser.JSONParser();
        //        if (input == null) break;
        //        newPlayer.RunCommand(input);
        //    }
        //    return;
        //}

        /// <Summary>
        /// Test harness for refree -- assign7
        /// </Summary>
        //public static void Main(String[] args)
        //{
        //    // Your component should assume that every sequence of commands 
        //    // consists of two Name messages, followed by two[Placement, Placement], 
        //    // followed by a number of[Worker, Directions]. 
        //    // The messages correspond to alternating interactions with the 
        //    // two players of a Santorini game.Your component is not responsible 
        //    // to reply to invalid messages including sequences of messages that 
        //    // do not follow the above pattern.

        //    JSONEncoder parser = new JSONEncoder();
        //    JToken input;
        //    Referee gameRef = new Referee();
        //    Board recordBoard = Board.InitBoard();

        //    // init players
        //    input = parser.JSONParser();
        //    gameRef.Name((string)input);
        //    Player one = new Player((string)input);
        //    input = parser.JSONParser();
        //    gameRef.Name((string)input);
        //    Player two = new Player((string)input);

        //    gameRef = new Referee(one, two);

        //    input = parser.JSONParser();
        //    gameRef.PlaceWorkers((JArray)input);
        //    input = parser.JSONParser();
        //    gameRef.PlaceWorkers((JArray)input);

        //    string workerName = "";
        //    string[] directions = new string[]{}; 

        //    // placement and execution
        //    while (true)
        //    {
        //        input = parser.JSONParser();
        //        if (input == null) break;
        //        //Console.WriteLine(input);
        //        //if (input.GetType().Equals(typeof(JValue))) // name
        //        //{
        //        //    Console.WriteLine("name:" + input);
        //        //}
        //        //if (input.GetType().Equals(typeof(JArray))) // placement, execution
        //        //{
        //        //    JArray temp = (JArray)input;
        //        //    workerName = (string)temp[0];
        //        //    directions = temp[1].ToObject<string[]>();
        //        //    Console.WriteLine("place/execute:" + input);
        //        //}
        //        //else
        //        //{
        //        //    Console.WriteLine("---------unexpected type for input");
        //        //}
        //        workerName = (string)input[0];
        //        directions = input[1].ToObject<string[]>();
        //        if (gameRef.ExecutePlay(workerName, directions) == null)
        //            break;
        //    }
        //    return;
        //}

        /// <summary>
        /// Game admin - currently working as parser for proxy player
        /// </summary>
        public static void Main(String[] args)
        {
            // read the text from input files
            // send them over to remote via tcp
            JSONEncoder parser = new JSONEncoder();
            int port = 8000;
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ipAddr, port);
            server.Start();
            Console.WriteLine("Waiting for connection....");

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("connected!");

            NetworkStream stream = client.GetStream();

            // Buffer for reading data
            byte[] readBytes = new byte[4096];
            JArray input;

            while (true)
            {
                // read from input file and send it via tcp connection
                input = (JArray)parser.JSONParser();
                Console.WriteLine("--------------");
                Console.WriteLine(JSONEncoder.DumpJson(input));
                if (input == null) break;
                // convert input to byte[]
                byte[] temp = Encoding.ASCII.GetBytes(JSONEncoder.DumpJson(input));
                stream.Write(temp, 0, temp.Length);
                stream.Flush();

                int i = 0;
                while ((i = stream.Read(readBytes, 0, readBytes.Length)) != 0)
                {
                    string readData = Encoding.ASCII.GetString(readBytes, 0, i);
                    if (readData == "Santorini is broken")
                    {
                        Console.WriteLine(readData);
                        client.Close();
                        server.Stop();
                        return;
                    }
                    Console.WriteLine("received: {0}", readData);
                    stream.Flush();
                    break;
                }

                //int i = 0;
                //string readData = Encoding.ASCII.GetString(readBytes, 0, i);
                //Console.WriteLine("received: {0}", readData);
                //stream.Flush();
            }
            client.Close();
            server.Stop();
        }
    }
}
