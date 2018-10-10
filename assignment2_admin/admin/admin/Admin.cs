using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace admin
{
    public class Admin
    {
        // Admin state
        int adminNumber = -1;
        public int playerNumber = -1;
        public bool playerSwap = false;

        // Game state
        bool gameStart = false;
        bool swapMade = false;
        bool adminWinner = true; // true for Admin, false for Player
        bool playerWinner = false; // true for Player, false for Admin

        JSONEncoder JSONEcoder = new JSONEncoder();

        public int AdminNumber { get => adminNumber; }

        public void DeclareAdminNumber()
        {
            adminNumber = 8; // fixed value for testing
        }



        public void StartGame()
        {
            DeclareAdminNumber();

            SendMessage("playerPickNumber");
            playerNumber = GetPlayerNumber();
            gameStart = true;

            SendMessage("playerCardSwap");
            playerSwap = GetPlayerSwap();
            swapMade = true;

            dynamic[] args = { AdminNumber };
            SendMessage("playerCheckIsWinner", args);
            playerWinner = GetIsPlayerWinner();
            adminWinner = IsAdminWinner();

            if (playerWinner != adminWinner)
            {
                Console.WriteLine("fair game. Good Bye");
                EndGame();
            }
            else
            {
                Console.WriteLine("Cheating.");
                System.Environment.Exit(1);
            }

        }

        public bool GetIsPlayerWinner()
        {
            JToken JSONResponse = JSONEcoder.JSONParser();
            playerWinner = (bool)JSONResponse;
            return playerWinner;
        }

        public bool IsAdminWinner()
        {
            adminWinner = (adminNumber > playerNumber) ? true : false;
            // need to take care of swapMade boolean
            adminWinner = (playerSwap) ? !adminWinner : adminWinner;
            if (adminWinner) 
            {
                Console.WriteLine("Admin Wins");
            }
            else{
                Console.WriteLine("Player Wins");
            }
            return adminWinner;
        }

        public int GetPlayerNumber()
        {
            if (gameStart)
            {
                Console.WriteLine("Player can't change number during a game");
                // exit here
                System.Environment.Exit(1);
            }
            JToken JSONResponse = JSONEcoder.JSONParser();
            playerNumber = (int)JSONResponse;
            //Console.WriteLine(playerNumber);
            return playerNumber;
        }

        public bool GetPlayerSwap()
        {
            if (swapMade)
            {
                Console.WriteLine("Player can't change card more than once");
                // exit here
                System.Environment.Exit(1);
            }
            JToken JSONReponse = JSONEcoder.JSONParser();
            playerSwap = (bool)JSONReponse;
            //Console.WriteLine(playerSwap);
            return playerSwap;
        }

        public void EndGame()
        {
            adminNumber = -1;
            System.Environment.Exit(0);
            // exit here
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
