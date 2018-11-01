using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            JToken JSONcommand = null;
            Player newPlayer = new Player();

            // Dispatch
            while (true)
            {
                JSONcommand = JSONEncoder.JSONParser();
                if (JSONcommand == null) break;
                JObject command = JObject.Parse(JSONcommand.ToString());
                newPlayer.RunCommand(command);
            }

            return;
        }
    }
}
