using System;
using System.Collections.Generic;
using System.Reflection;
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

            // E2E_testing



            // dispatch -- reading from STDIN
            while (true)
            {
                input = parser.JSONParser();
                if (input == null) break;
                // validate board
                Board newBoard = new Board((JArray)input[0]);

                if (!RuleChecker.IsBoardValid())
                {
                    Console.WriteLine("no");
                    continue;
                }

                // validate move
                    string worker = (string)input[1];
                JArray directions = (JArray)input[2];
                string[] stringDirections = directions.ToObject<string[]>();
                if (!RuleChecker.IsValidMove(worker, stringDirections))
                {
                    Console.WriteLine("no");
                    continue;
                }
                Board.Board_ = Board.Move(worker, stringDirections[0]);

                // validate build
                if (!RuleChecker.IsValidBuild(worker, stringDirections))
                {
                    Console.WriteLine("no");
                    continue;
                }

                // all good
                Console.WriteLine("yes");
            }
            return;
        }
    }
}
