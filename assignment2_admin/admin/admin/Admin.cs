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
            try
            {
                playerWinner = Convert.ToBoolean(JSONResponse.ToString());

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid attempt: bool convert error");
                System.Environment.Exit(1);
            }

            return playerWinner;
        }

        public bool IsAdminWinner()
        {
            adminWinner = (adminNumber > playerNumber) ? true : false;
            // take account of playerSwap boolean
            adminWinner = (playerSwap) ? !adminWinner : adminWinner;
            Console.WriteLine(adminWinner ? "Admin wins" : "Player Wins");
            return adminWinner;
        }

        public int GetPlayerNumber()
        {
            if (gameStart)
            {
                Console.WriteLine("Player can't change number during a game");
                System.Environment.Exit(1);
            }
            JToken JSONResponse = JSONEcoder.JSONParser();
            try
            {
                playerNumber = Convert.ToInt32(JSONResponse.ToString());
                if (playerNumber < 1 || playerNumber > 10)
                {
                    Console.WriteLine("Inavlid input: 1 to 10");
                    System.Environment.Exit(1);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Inavlid atempt: int convert error");
                System.Environment.Exit(1);
            }

            return playerNumber;
        }

        public bool GetPlayerSwap()
        {
            if (swapMade)
            {
                Console.WriteLine("Player can't change card more than once");
                System.Environment.Exit(1);
            }
            JToken JSONReponse = JSONEcoder.JSONParser();
            try
            {
                playerSwap = Convert.ToBoolean(JSONReponse.ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid attempt: bool convert error");
                System.Environment.Exit(1);
            }

            return playerSwap;
        }

        public void EndGame()
        {
            adminNumber = -1;
            System.Environment.Exit(0);
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
