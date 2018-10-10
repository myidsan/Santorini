using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace admin
{
    public class Admin
    {
        int adminNumber = -1;
        public int playerNumber = -1;
        public bool playerSwap = false;

        public int AdminNumber { get => adminNumber; }

        public void DeclareAdminNumber()
        {
            adminNumber = 8; // fixed value for testing
        }



        public void StartGame()
        {
            DeclareAdminNumber();
            SendMessage("playerPickNumber");
            // waits for the Player input --
            //dynamic[] args = { 2, "string" };
            //SendMessage("IntArgs", args);
        }

        public void EndGame()
        {
            adminNumber = -1;
        }

        // helper methods
        public static void SendMessage(string operationName)
        {
            Dictionary<string, string> command = new Dictionary<string, string>
            {
                { "operation-name", operationName }
            };
            Console.WriteLine(JsonConvert.SerializeObject(command));
        }


        public static void SendMessage(string operationName, dynamic[] args)
        {
            Dictionary<string, dynamic> command = new Dictionary<string, dynamic>
            {
                { "operation-name", operationName }
            };
            for (int i = 0; i < args.Length; i++)
            {
                command.Add("operation-argument" + (i+1), args[i]);
            }
            Console.WriteLine(JsonConvert.SerializeObject(command));
        }
    }
}
