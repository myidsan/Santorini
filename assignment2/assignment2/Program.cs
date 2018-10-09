using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace assignment2
{
    class Program
    {
     static Player newPlayer = new Player();

        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            JToken command = null;

            // Dispatch
            while (true) 
            {
                command = JSONEcoder.JSONParser();
                if (command == null) break;
                JObject JSONcommand = JObject.Parse(command.ToString());
                string operationName = JSONcommand["operation-name"].ToString();
                RunCommand(operationName, JSONcommand);
            }

            Console.WriteLine("End of Game");
            return;
        }

        static void RunCommand(string command, JObject json)
        {
            switch (command)
            {
                case "DeclareNumber":
                    int num = (int) json["operation-argument1"];
                    newPlayer.DeclareNumber(num);
                    break;

                case "GetNumber":
                    newPlayer.GetNumber();
                    break;

                case "SwitchNumber":
                    bool choice = (bool)json["operation-argument1"];
                    newPlayer.SwitchNumber(choice);
                    break;

                case "HasPlayerSwitched":
                    newPlayer.HasPlayerSwitched();
                    break;

                default:
                    Console.WriteLine("default case");
                    break;
            }
            return;
        }
    }
}
