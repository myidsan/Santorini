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

            // dispatch
            while (true)
            {
                input = parser.JSONParser();
                if (input == null) break;

                Board newBoard = new Board((JArray)input[0]);
                JArray action = (JArray)input[1];
                newBoard.RunCommand(action);
                newBoard.PrintBoard();
            }

            Console.WriteLine("terminating the program successfully...");
            return;
        }
    }
}
