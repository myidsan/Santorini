using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            JToken command = null;
            Player newPlayer = new Player();

            // Dispatch
            while (true) 
            {
                command = JSONEncoder.JSONParser();
                if (command == null) break;
                JObject JSONcommand = JObject.Parse(command.ToString());
                Player.RunCommand(JSONcommand);
            }

            return;
        }
    }
}
