using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace admin
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            JToken JSONcommand = null;
            Admin newAdmin = new Admin();
            newAdmin.StartGame();

            // Dispatch
            while (newAdmin.AdminNumber != -1) // game started && 
            {
                JSONcommand = JSONEncoder.JSONParser();
                if (JSONcommand == null) break;
                JObject command = JObject.Parse(JSONcommand.ToString());
            }

            return;
        }
    }
}
