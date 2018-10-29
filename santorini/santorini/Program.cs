using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Program
    {
        /// <summary>
        ///  Test harness for the board
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        //public static void Main(string[] args)
        //{
        //    JSONEncoder parser = new JSONEncoder();
        //    JToken input;

        //    // dispatch
        //    while (true)
        //    {
        //        input = parser.JSONParser();
        //        if (input == null) break;
        //        Board newBoard = new Board((JArray)input[0]);
        //        //newBoard.PrintBoard();

        //        JArray action = (JArray)input[1];
        //        newBoard.RunCommand(action);
        //    }
        //    return;
        //}

        /// <summary>
        /// Test harness for the rule checker
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            JSONEncoder parser = new JSONEncoder();
            //RuleChecker checker = new RuleChecker();
            JToken input;

            // dispatch -- reading from STDIN
            while (true)
            {
                input = parser.JSONParser();
                if (input == null) break;
                // validate board
                Board newBoard = new Board((JArray)input[0]);
                Cell[,] orignalBoard = newBoard.Board_;

                Board tempBoardObj;
                Cell[,] tempBoard;

                string JSONresult;

                if (!RuleChecker.IsBoardValid(newBoard))
                {
                    JSONresult = JsonConvert.SerializeObject("no");
                    Console.WriteLine(JSONresult);
                    continue;
                }

                // validate move
                string worker = (string)input[1];
                JArray directions = (JArray)input[2];
                string[] stringDirections = directions.ToObject<string[]>();
                if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
                {
                    JSONresult = JsonConvert.SerializeObject("no");
                    Console.WriteLine(JSONresult);
                    continue;
                }

                // make a tempBoard and 
                // construct a new tempBoardObj for build
                tempBoard = newBoard.Move(worker, stringDirections[0]);
                tempBoardObj = new Board(tempBoard, newBoard.PlayerPosition);

                // check whether the player has won
                // if so, terminate the game
                if (RuleChecker.IsPlayerWinner(tempBoardObj, worker))
                {
                    Console.WriteLine("worker is on level 3: player wins");
                    continue;
                }

                // validate build
                if (!RuleChecker.IsValidBuild(tempBoardObj, worker, stringDirections))
                {
                    JSONresult = JsonConvert.SerializeObject("no");
                    Console.WriteLine(JSONresult);
                    continue;
                }
                tempBoard = tempBoardObj.Move(worker, stringDirections[0]);

                // all good
                JSONresult = JsonConvert.SerializeObject("yes");
                Console.WriteLine(JSONresult);
            }
            return;
        }
    }
}
