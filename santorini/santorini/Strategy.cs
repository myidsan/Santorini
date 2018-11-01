﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Strategy
    {
        public Strategy()
        {
        }

        // encapsulates WinInOneTurn() and PreventLoseInOneTurn() for the user
        public static ArrayList GetOneMoveWinStrategy(Board board, string playerColor, string oppColor)
        {
            ArrayList simpleWin = WinInOneTurn(board, playerColor);

            if (simpleWin.Count != 0)
            {
                return simpleWin;
            }
            ArrayList preventSimpleWin = PreventLoseInOneTurn(board, playerColor, oppColor);
            return preventSimpleWin;
        }

        //public static List<List<object>> WinInOneTurn(Board board, string color)
        public static ArrayList WinInOneTurn(Board board, string color)
        {
            // get the coordinate location of the color players
            // check all eight directions of them to see if the height == 3
            // if true, add the move direction to the directions[] -- one element case
            // append { worker, { move_dir } }

            List<string> targets = Player.GetPlayerWorkers(board, color); // { "myColor1", "myColor2" }

            ArrayList OneTurnWinPlay = new ArrayList();
            foreach (var workerName in targets)
            {
                foreach (var moveDir in board.directions.Keys)
                {
                    if (board.NeighboringCellExists(workerName, moveDir))
                    {
                        List<int> AfterMoveCoordinate = board.GetDesiredPosition(workerName, moveDir);
                        if (board.Board_[AfterMoveCoordinate[0], AfterMoveCoordinate[1]].Height == 3 &&
                            RuleChecker.IsValidMove(board, workerName, new string[]{moveDir}))
                        {
                            // winning scenario with only moveDir
                            OneTurnWinPlay.Add(new ArrayList { workerName, new ArrayList { moveDir } });
                        }
                    }
                }
            }
            return OneTurnWinPlay;
        }

        public static ArrayList PreventLoseInOneTurn(Board board, string color, string oppColor)
        {
            /// 1. Run the WinInOneTurn for the oppWorkers
            /// 3. calculate the targetCellPos (List<int>) by oppWorkerPos + moveDirection
            /// 4. calculate the "path" from player worker to the targetCellPos by playerWorkerPos - targetCellPos
            /// 5. in a for loop of all moveDirections, path - moveDir and lookup the List<int> key in Board.reverseDirection
            ///      if containsKey, get the value of the key as buildDir and add { workerName, { moveDir, buildDir } }
            ///      if not, continue
            ArrayList validMoves = new ArrayList(){};
            List<string> targets = Player.GetPlayerWorkers(board, color); // { "myColor1", "myColor2" }

            ArrayList oppWorkerOneTurnWin = WinInOneTurn(board, oppColor);
            if (oppWorkerOneTurnWin.Count != 0)
            {
                foreach (ArrayList oppPlay in oppWorkerOneTurnWin)
                {
                    // debug
                    var worker = oppPlay[0].ToString();
                    var moveDir = ((ArrayList)oppPlay[1])[0].ToString();
                   
                    List<int> targetCellPos = board.GetDesiredPosition(worker, moveDir);
                    //Console.WriteLine("targetCellPos: ");
                    //targetCellPos.ForEach(Console.WriteLine);


                    foreach (var workerName in targets)
                    {
                        List<int> path = GetPossiblePath(board, workerName, targetCellPos);
                        //Console.WriteLine("path: ");
                        //path.ForEach(Console.WriteLine);
                        if (path.Count != 0)
                        {
                            foreach (var play in CanExecutePathInOnePlay(board, workerName, path))
                            {
                                validMoves.Add(new ArrayList { workerName, play });
                            }
                        }
                        //Console.WriteLine("----END for one player worker----");
                    }
                }
            }
            return validMoves;
        }

        // return - list of possible paths: List<List<int>>
        public static List<int> GetPossiblePath(Board board, string worker, List<int> targetCellPos)
        {
            //Console.WriteLine("in GetPossiblePath");
            List<int> path = new List<int>();
            double MAX_DISTANCE_FOR_ONE_PLAY = 8.0;
            double distance = 0;
            List<int> workerPos = board.PlayerPosition[worker];
            // calculate path (distance)
            for (int i = 0; i < workerPos.Count; i++)
            {
                path.Add(targetCellPos[i] - workerPos[i]);
                distance += Math.Pow(path[i], 2);
            }
            if (Math.Sqrt(distance) > Math.Sqrt(MAX_DISTANCE_FOR_ONE_PLAY))
            {
                //Console.WriteLine("quitting cuz too far");

                return new List<int> { };
            }
            return path;

        }

        // adding 10/30/2018 for Strategy class
        // assumes that the coordinate will be not out of bounds
        public static ArrayList CanExecutePathInOnePlay(Board board, string workerName, List<int> path)
        {
            //Console.WriteLine("CanExecutePathInOnePlay:");
            ArrayList result = new ArrayList();
            foreach (var moveDir in board.directions.Keys)
            {
                List<int> key = new List<int>();
                for (int i = 0; i < path.Count; i++)
                {
                    key.Add(path[i] - board.directions[moveDir][i]);

                }
                // need to take care of out of bound
                if (!RuleChecker.IsValidMove(board, workerName, new string[] { moveDir })) continue;

                foreach (string buildDir in board.directions.Keys)
                {
                    if (key.SequenceEqual(board.directions[buildDir]))
                    {
                        //Console.WriteLine("in arraylist equals:");
                        //Console.WriteLine("key:");
                        //key.ForEach(Console.WriteLine);
                        //Console.WriteLine("buildDir:");
                        result.Add(new ArrayList { moveDir, buildDir });
                    }
                }

                // List<> or ArrayList doesn't override Equals -- question
                //if (board.reverseDirections.ContainsKey(key))
                //{
                //    Console.WriteLine("contains key");
                //    string buildDir = board.reverseDirections[key];
                //    result.Add(new List<string> { moveDir, buildDir });
                //}
            }

            return result;
        }

        // should return all possible moves for both playerWorkers
        // check for NeighborCellExists and Ouccpied
        public static ArrayList DefaultPlay(Board board, string playerColor)
        {
            ArrayList validMoves = new ArrayList() { };
            List<string> targets = Player.GetPlayerWorkers(board, playerColor); // { "myColor1", "myColor2" }
            foreach (var workerName in targets)
            {
                foreach (string moveDir in board.directions.Keys)
                {
                    // creats a clean copy of the board
                    JArray test = new JArray();
                    test = board.DumpBoard();
                    Board hell = new Board(test);

                    if (!RuleChecker.IsValidMove(hell, workerName, new string[] { moveDir }))
                    {
                        continue;
                    }
                    Cell[,] temp = hell.Move(workerName, moveDir);
                    Board tempBoard = new Board(temp, hell.PlayerPosition);

                    List<int> pos = hell.PlayerPosition[workerName];

                    foreach (string buildDir in tempBoard.directions.Keys)
                    {
                        if (!RuleChecker.IsValidBuild(tempBoard, workerName, new string[] { moveDir, buildDir }))
                        {
                            continue;
                        }
                        validMoves.Add(new ArrayList { workerName, new ArrayList { moveDir, buildDir } });
                    }

                }
            }
            //Console.WriteLine(validMoves.Count);
            return validMoves;
        }
    }
}
