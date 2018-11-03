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


                //    if (stringDirections.Length == 1)
                //    {
                //        if (RuleChecker.IsValidMove(newBoard, worker, stringDirections))
                //        {
                //            tempBoard = newBoard.Move(worker, stringDirections[0]);
                //            if (RuleChecker.IsPlayerWinner(newBoard, worker))
                //            {
                //                JSONresult = JsonConvert.SerializeObject("yes");
                //                Console.WriteLine(JSONresult);
                //                continue;
                //            }
                //            else
                //            {
                //                JSONresult = JsonConvert.SerializeObject("no");
                //                Console.WriteLine(JSONresult);
                //                continue;
                //            }
                //        }
                //        else
                //        {
                //            JSONresult = JsonConvert.SerializeObject("no");
                //            Console.WriteLine(JSONresult);
                //            continue;
                //        }
                //    }
                //    else
                //    {
                //        // length == 2 and not valid move
                //        if (!RuleChecker.IsValidMove(newBoard, worker, stringDirections))
                //        {
                //            JSONEncoder.PrintJson("no");
                //            continue;
                //        }

                //        // valid move, check valid move with a new tempBoard
                //        tempBoard = newBoard.Move(worker, stringDirections[0]);
                //        // make a tempBoard and move
                //        JArray test = new JArray();
                //        test = newBoard.DumpBoard();
                //        tempBoardObj = new Board(test);
                //        tempBoard = tempBoardObj.Move(worker, stringDirections[0]);

                //        if (!RuleChecker.IsValidBuild(tempBoardObj, worker, stringDirections))
                //        {
                //            JSONresult = JsonConvert.SerializeObject("no");
                //            Console.WriteLine(JSONresult);
                //            continue;
                //        }

                //        // all good
                //        JSONresult = JsonConvert.SerializeObject("yes");
                //        Console.WriteLine(JSONresult);
                //    }

                //}
                //return;
                //}

                //////// assignment 5
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
    }
}
