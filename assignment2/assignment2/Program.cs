using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace assignment2
{
    class Program
    {
        static int num = 0;
        static Player newPlayer = new Player();

        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            Queue<JToken> commands = JSONEcoder.JSONParser();
            Console.WriteLine(commands);

            // parser
            while (commands.Count > 0)
            {
                var command = commands.Dequeue().ToString();
                var JSONcommand = JObject.Parse(command);
                string operationName = JSONcommand["operation-name"].ToString();
                Console.WriteLine(operationName);
                RunCommand(operationName);
            }
          
            
            return;
        }

        static void RunCommand(string command)
        {
            switch (command)
            {
                case "DeclareNumber":
                    Console.WriteLine("DeclareNumber case");
                    Console.WriteLine("Choose a number between 1 to 10");
                    newPlayer.DeclareNumber(num);
                    break;

                case "GetNumber":
                    Console.WriteLine("GetNumber case");
                    break;

                case "SwitchNumber":
                    Console.WriteLine("SwitchNumber case");
                    break;

                case "HasPlayerSwitched":
                    Console.WriteLine("HasPlayerSwitched case");
                    break;

                default:
                    Console.WriteLine("default case");
                    break;
            }
            return;
        }
    }
}
