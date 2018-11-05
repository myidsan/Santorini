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
        ///  Test harness for the board -- assign3
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
        /// Test harness for the rule checker -- assign4
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

                if (stringDirections.Length == 1)
                {
                    if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
                    {
                        JSONEncoder.PrintJson("no");
                        continue;
                    }
                    newBoard.Move(worker, stringDirections[0]);
                    if (!RuleChecker.IsPlayerWinner(newBoard, worker))
                    {
                        JSONEncoder.PrintJson("no");
                        continue;
                    }
                }
                else
                {
                    if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
                    {
                        JSONEncoder.PrintJson("no");
                        continue;
                    }
                    tempBoard = newBoard.Move(worker, stringDirections[0]);
                    JArray test = new JArray();
                    test = newBoard.DumpBoard();
                    tempBoardObj = new Board(test);
                    if (!RuleChecker.IsValidBuild(tempBoardObj, worker, stringDirections))
                    {
                        JSONEncoder.PrintJson("no");
                        continue;
                    }
                }
                JSONEncoder.PrintJson("yes");
            }
        }

        /// <Summary>
        /// Test harness for the rule checker -- assign5
        /// </Summary>
        //public static void Main(string[] args)
        //{
        //    JSONEncoder parser = new JSONEncoder();

        //    JArray input = parser.JSONParser();
        //    string color = input[1].ToString();
        //    Player newPlayer = new Player(color);
        //    newPlayer.RunCommand(input);

        //    // dispatch
        //    while (true)
        //    {
        //        input = parser.JSONParser();
        //        if (input == null) break;
        //        newPlayer.RunCommand(input);
        //    }
        //    return;
        //}
    }
}
