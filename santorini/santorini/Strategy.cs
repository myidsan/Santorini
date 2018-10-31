using System;
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
        public static List<dynamic> GetOneMoveWinStrategy(Board board, string color)
        {
            return new List<dynamic>();
        }

        //public static List<List<object>> WinInOneTurn(Board board, string color)
        public static ArrayList WinInOneTurn(Board board, string color)
        {
            // get the coordinate location of the color players
            // check all eight directions of them to see if the height == 3
            // if true, add the move direction to the directions[] -- one element case
            // append { worker, { move_dir } }

            List<string> targets = new List<string>(); // { "White1", White2" }

            foreach (var workerName in board.PlayerPosition.Keys)
            {
                if (workerName.Contains(color))
                {
                    targets.Add(workerName);
                }
            }

            ArrayList OneTurnWinPlay = new ArrayList();
            foreach (var workerName in targets)
            {
                foreach (var moveDir in board.directions.Keys)
                {
                    if (board.NeighboringCellExists(workerName, moveDir))
                    {
                        List<int> AfterMoveCoordinate = board.GetDesiredPosition(workerName, moveDir);
                        if (board.Board_[AfterMoveCoordinate[0], AfterMoveCoordinate[1]].Height == 3)
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
            ArrayList validMoves = new ArrayList();
            List<string> targets = new List<string>(); // { "myColor1", "myColor2" }

            foreach (var workerName in board.PlayerPosition.Keys)
            {
                if (workerName.Contains(color))
                {
                    targets.Add(workerName);
                }
            }

            ArrayList oppWorkerOneTurnWin = WinInOneTurn(board, oppColor);
            if (oppWorkerOneTurnWin.Count != 0)
            {
                foreach (ArrayList oppPlay in oppWorkerOneTurnWin)
                {
                    // debug
                    string JSONresult = JsonConvert.SerializeObject(oppPlay);
                    Console.WriteLine(JSONresult);
                    // parsing
                    var worker = oppPlay[0].ToString();
                    var moveDir = ((ArrayList)oppPlay[1])[0].ToString();
                    //Console.WriteLine(worker + "-----");
                    //Console.WriteLine(moveDir + "-----");
                    List<int> targetCellPos = board.GetDesiredPosition(worker, moveDir);
                    Console.WriteLine("targetCellPos: ");
                    targetCellPos.ForEach(Console.WriteLine);


                    foreach (var workerName in targets)
                    {
                        List<int> path = GetPossiblePath(board, workerName, targetCellPos);
                        Console.WriteLine("path: ");
                        path.ForEach(Console.WriteLine);
                        if (path.Count != 0)
                        {
                            foreach (var item in CanExecutePathInOnePlay(board, workerName, path))
                            {
                                validMoves.Add(new ArrayList { workerName, item });
                            }
                        }
                        Console.WriteLine("----END for one player worker----");
                    }
                }
            }
            return validMoves;
        }

        // return - list of possible paths: List<List<int>>
        public static List<int> GetPossiblePath(Board board, string worker, List<int> targetCellPos)
        {
            Console.WriteLine("in GetPossiblePath");
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
                Console.WriteLine("quitting cuz too far");
                return new List<int> { };
            }
            return path;

        }

        // adding 10/30/2018 for Strategy class
        // assumes that the coordinate will be not out of bounds
        public static ArrayList CanExecutePathInOnePlay(Board board, string workerName, List<int> path)
        {
            Console.WriteLine("CanExecutePathInOnePlay:");
            ArrayList result = new ArrayList();
            foreach (var moveDir in board.directions.Keys)
            {
                List<int> key = new List<int>();
                for (int i = 0; i < path.Count; i++)
                {
                    key.Add(path[i] - board.directions[moveDir][i]);

                }
                // need to take care of out of bound
                if (!board.NeighboringCellExists(workerName, moveDir)) continue;

                foreach (string buildDir in board.directions.Keys)
                {
                    if (key.SequenceEqual(board.directions[buildDir]))
                    {
                        Console.WriteLine("in arraylist equals:");
                        Console.WriteLine("key:");
                        key.ForEach(Console.WriteLine);
                        Console.WriteLine("buildDir:");
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
    }
}
