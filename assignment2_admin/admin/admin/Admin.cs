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
            SendMessage("IsPlayerWinner", args);
            playerWinner = GetIsPlayerWinner();
            adminWinner = IsAdminWinner();

            if (playerWinner != adminWinner)
            {
                Console.WriteLine("fair game");
                EndGame();
            }
            else
            {
                Console.WriteLine("Cheating");
                System.Environment.Exit(1);
            }

        }

        bool GetIsPlayerWinner()
        {
            JToken JSONResponse = JSONEcoder.JSONParser();
            try
            {
                if (JSONResponse.Type == JTokenType.Boolean)
                {
                    playerWinner = Convert.ToBoolean(JSONResponse.ToString());
                }
                else System.Environment.Exit(1);
            }
            catch (Exception)
            {
                //Console.WriteLine("Invalid attempt: bool convert error");
                System.Environment.Exit(1);
            }

            return playerWinner;
        }

        bool IsAdminWinner()
        {
            adminWinner = (adminNumber > playerNumber) ? true : false;
            // take account of playerSwap boolean
            adminWinner = (playerSwap) ? !adminWinner : adminWinner;
            Console.WriteLine(adminWinner ? "Admin wins" : "Player Wins");
            return adminWinner;
        }

        int GetPlayerNumber()
        {
            if (gameStart)
            {
                //Console.WriteLine("Invalid attempt: Player can't change number during a game");
                System.Environment.Exit(1);
            }
            JToken JSONResponse = JSONEcoder.JSONParser();
            try
            {
                if (JSONResponse.Type == JTokenType.Integer)
                { 
                    playerNumber = Convert.ToInt32(JSONResponse.ToString());
                    playerNumber = JSONResponse.ToObject<int>();
                    if (playerNumber < 1 || playerNumber > 10)
                    {
                        //Console.WriteLine("Inavlid input: 1 to 10");
                        System.Environment.Exit(1);
                    }
                }
                else System.Environment.Exit(1);
            }
            catch (Exception)
            {
                //Console.WriteLine("Inavlid attempt: int convert error");
                System.Environment.Exit(1);
            }

            return playerNumber;
        }

        bool GetPlayerSwap()
        {
            if (swapMade)
            {
                //Console.WriteLine("Invalid attempt: Player can't change card more than once");
                System.Environment.Exit(1);
            }
            JToken JSONResponse = JSONEcoder.JSONParser();
            try
            {
                if (JSONResponse.Type == JTokenType.Boolean)
                {
                    playerSwap = Convert.ToBoolean(JSONResponse.ToString());
                }
                else System.Environment.Exit(1);

            }
            catch (Exception)
            {
                //Console.WriteLine("Invalid attempt: bool convert error");
                System.Environment.Exit(1);
            }

            return playerSwap;
        }

        void EndGame()
        {
            adminNumber = -1;
            System.Environment.Exit(0);
        }

        // helper methods
        static void SendMessage(string operationName)
        {
            Dictionary<string, string> command = new Dictionary<string, string>
            {
                { "operation-name", operationName }
            };
            Console.WriteLine(JsonConvert.SerializeObject(command));
        }


        static void SendMessage(string operationName, dynamic[] args)
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
