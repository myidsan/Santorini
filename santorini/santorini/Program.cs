using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace santorini
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            JSONEncoder parser = new JSONEncoder();

            JToken input;

            //newBoard.PrintBoard();

            //newBoard.PlaceWorker("blue1", 0, 1);
            //newBoard.PlaceWorker("white2", 2, 2);
            //newBoard.PlaceWorker("blue2", 3, 4);

            //newBoard.Build("white1", "S");
            //newBoard.NeighboringCellExists("white1", "W"); // false
            //newBoard.NeighboringCellExists("white2", "W"); // True

            // dispatch
            while (true)
            {
                input = parser.JSONParser();
                if (input == null) break;

                Console.WriteLine("before sample");
                Board newBoard = new Board((JArray)input.First);
                //newBoard.PlaceWorker("white1", 0, 0);
                newBoard.NeighboringCellExists("white1", "W"); // false
                newBoard.PrintBoard();
                newBoard.Occupied("white1", "S"); // false
                //newBoard.Occupied("white1", "N"); // Exception for NeightboringCellExists


                //Console.WriteLine("board {0}");
                //var board = input.Value;
                //Console.WriteLine(input.GetType()); // JValue
                //Console.WriteLine(board.GetType()); // String
                //Console.WriteLine(board); // type: string
                //Board sample = new Board(board);


                //JObject command = JObject.Parse(input.ToString());
                //Console.WriteLine(command);
            }

            Console.WriteLine("terminating the program successfully...");
            return;

            //newBoard.Occupied("white2", "NW"); // false
            //newBoard.PrintBoard();
            //newBoard.Move("white2", "W"); // {2,1}
            //newBoard.Move("blue1", "W");
            //newBoard.PrintBoard();
            //newBoard.Move("white1", "N"); // original board
            //newBoard.PrintBoard();
        }
    }
}
