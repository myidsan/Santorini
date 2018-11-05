using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace santorini
{
    public class Strategy
    {
        public Strategy()
        {
            StreamReader file = File.OpenText(@"./strategy.config");
            JsonTextReader reader = new JsonTextReader(file);
            JObject o2 = (JObject)JToken.ReadFrom(reader);
            int numPlays = Convert.ToInt32(o2["look-ahead"]);
        }

        private static ArrayList masterList = new ArrayList() { };

        public static ArrayList PreventLoseInNTurn(Board board, string playerColor, string oppColor, int numPlays)
        {
            //if (numPlays == 1)
            //{
            //    ArrayList hello = GetNextBestPlayStrategy(board, playerColor, oppColor);
            //    if (hello.Count != 0)
            //    {
            //        masterList.AddRange(hello);
            //    }
            //}
            //else
            //{
            //    if (WinInOneTurn(board, playerColor).Count != 0)
            //    {
            //        masterList.AddRange(WinInOneTurn(board, playerColor));
            //    }

            //    if (PreventLoseInOneTurn(board, playerColor, oppColor).Count != 0)
            //    {
            //        masterList.AddRange(PreventLoseInOneTurn(board, playerColor, oppColor));
            //    }
            //}

            while (numPlays > 1)
            {
                // need to create boards from intermediateList
                // feed each board to GetNextBestPlayStrategy
                // if empty, that means that the player cannot prevent the opp from winning in one move
                // --> return empty list
                // if not empty, 
                ArrayList innerList = DefaultPlay(board, oppColor);
                foreach (ArrayList play in innerList)
                {
                    string worker = (string)play[0];
                    string[] directions = ((IEnumerable)play[1]).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();

                    Board tempBoardObj = new Board(board.DumpBoard());
                    tempBoardObj.Move(worker, directions[0]);
                    tempBoardObj.Build(worker, directions[1]);
                    ArrayList tempList = PreventLoseInNTurn(tempBoardObj, playerColor, oppColor, numPlays - 1);
                    if (tempList.Count == 0) 
                        return new ArrayList() { };
                    else
                        return tempList;
                }

                numPlays--;
            }

            if (WinInOneTurn(board, playerColor).Count != 0)
            {
                masterList.AddRange(WinInOneTurn(board, playerColor));
            }

            if (PreventLoseInOneTurn(board, playerColor, oppColor).Count != 0)
            {
                masterList.AddRange(PreventLoseInOneTurn(board, playerColor, oppColor));
            }
            if (masterList.Count == 0)
            {
                masterList.AddRange(DefaultPlay(board, playerColor));
            }


            return masterList;
        }

        /// <summary>
        /// ADDED from the player -- should refactor if this stays here --  changed to static
        /// </summary>
        /// <returns>The next best play strategy.</returns>
        public static ArrayList GetNextBestPlayStrategy(Board board, string playerColor, string oppColor)
        {
            // player can win in one move
            if (Strategy.WinInOneTurn(board, playerColor).Count != 0)
                return Strategy.WinInOneTurn(board, playerColor);

            // player can prevent the opp from winning in one move
            if (Strategy.PreventLoseInOneTurn(board, playerColor, oppColor).Count != 0)
                return Strategy.PreventLoseInOneTurn(board, playerColor, oppColor);

            // player can't win and opp can't win in one move
            if (Strategy.WinInOneTurn(board, playerColor).Count == 0 && Strategy.WinInOneTurn(board, oppColor).Count == 0)
                return Strategy.DefaultPlay(board, playerColor);

            // player cannot prevent the opp from winning in one move
            return new ArrayList() { };
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
                foreach (string buildDir in board.directions.Keys)
                {
                    // need to take care of out of bound
                    if (!RuleChecker.IsValidMove(board, workerName, new string[] { moveDir, buildDir })) continue;
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

                    foreach (string buildDir in board.directions.Keys)
                    {
                        JArray test = new JArray();
                        test = board.DumpBoard();
                        Board hell = new Board(test);
                        if (!RuleChecker.IsValidMove(hell, workerName, new string[] { moveDir, buildDir }))
                        {
                            continue;
                        }
                        Cell[,] temp = hell.Move(workerName, moveDir);
                        Board tempBoard = new Board(temp, hell.PlayerPosition);

                        List<int> pos = hell.PlayerPosition[workerName];
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
