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
        }

        static bool defaultCase = false;
        private static int numPlays = 2;

        // override method for testing purpose
        public static ArrayList PreventLoseInNTurn(Board board, string playerColor, string oppColor, int numPlays)
        {
            ArrayList masterList = new ArrayList() { };
            masterList.AddRange(GetNextBestPlayStrategy(board, playerColor, oppColor));
            Console.WriteLine(numPlays + ": " + masterList.Count);
            //Console.WriteLine("master: " + JSONEncoder.DumpJson(masterList));

            return PreventLoseInNTurnHelper(board, playerColor, oppColor, numPlays, masterList);
        }

        public static ArrayList PreventLoseInNTurn(Board board, string playerColor, string oppColor)
        {
            //string currentDirectory = Directory.GetCurrentDirectory();
            //string filePath = System.IO.Path.Combine(currentDirectory, "strategy.config");
            //StreamReader file = File.OpenText(filePath);
            //JsonTextReader reader = new JsonTextReader(file);
            //JObject o2 = (JObject)JToken.ReadFrom(reader);
            //numPlays = Convert.ToInt32(o2["look-ahead"]);

            ArrayList masterList = new ArrayList() { };
            masterList.AddRange(GetNextBestPlayStrategy(board, playerColor, oppColor));

            return PreventLoseInNTurnHelper(board, playerColor, oppColor, numPlays, masterList);
        }

        public static ArrayList PreventLoseInNTurnHelper(Board board, string playerColor, string oppColor, int numPlays, ArrayList tempList)
        {
            //Console.WriteLine(numPlays + "-exec: " + tempList.Count);
            if (numPlays != 0)
            {
                while (numPlays > 1)
                {
                    // need to create boards from intermediateList
                    // feed each board to GetNextBestPlayStrategy
                    // if empty, that means that the player cannot prevent the opp from winning in one move
                    // --> return empty list
                    // if not empty, 
                    ArrayList innerList = new ArrayList() { };
                    innerList.AddRange(tempList);
                    //Console.WriteLine("innerList" + numPlays + ": " + innerList.Count);

                    foreach (ArrayList play in innerList)
                    {
                        string worker = (string)play[0];
                        string[] directions = ((IEnumerable)play[1]).Cast<object>()
                                     .Select(x => x.ToString())
                                     .ToArray();
                        //Console.WriteLine(worker + " " + JSONEncoder.DumpJson(directions));

                        // player win in the current move, keep it
                        if (directions.Length == 1)
                        {
                            continue;
                        }

                        Board tempBoardObj1 = new Board(board.DumpBoard());
                        tempBoardObj1.Move(worker, directions[0]);

                        Board tempBoardObj2 = new Board(tempBoardObj1.DumpBoard());
                        tempBoardObj2.Build(worker, directions[1]);

                        //check if current play can result into player dying in opp's next move
                        //if so, remove current play from the list
                        //this makes the runtime a lot faster - by ignoring possible "smart moves" of the player
                        var hell = WinInOneTurn(tempBoardObj2, oppColor);
                        if (hell.Count != 0)
                        {
                            tempList.Remove(play);
                            continue;
                        }

                        // need to make the opp play valid moves
                        ArrayList oppList = GetNextBestPlayStrategy(tempBoardObj2, oppColor, playerColor);
                        //Console.WriteLine(numPlays + "-oppList: " + oppList.Count);

                        ArrayList perm = new ArrayList();

                        foreach (ArrayList oppPlay in oppList)
                        {
                            string oppWorker = (string)oppPlay[0];
                            string[] oppDirections = ((IEnumerable)oppPlay[1]).Cast<object>()
                                         .Select(x => x.ToString())
                                         .ToArray();

                            // opponent wins in the next move in current board
                            if (oppDirections.Length == 1)
                            {
                                tempList.Remove(play);
                                continue;
                            }

                            //Console.WriteLine(oppWorker + " " + JSONEncoder.DumpJson(oppDirections));

                            Board tempBoardObj3 = new Board(tempBoardObj2.DumpBoard());
                            tempBoardObj3.Move(oppWorker, oppDirections[0]);

                            Board tempBoardObj4 = new Board(tempBoardObj3.DumpBoard());
                            tempBoardObj4.Build(oppWorker, oppDirections[1]);

                            // check if current play can result into player dying in opp's next move
                            // if so, remove current play from the list
                            // this makes the runtime a lot faster - by ignoring possible "smart moves" of the player

                            //if (PreventLoseInOneTurn(tempBoardObj4, playerColor, oppColor).Count == 0 &&
                            //WinInOneTurn(tempBoardObj4, oppColor).Count != 0)

                            // opp can win because of its next move remove the play
                            if (WinInOneTurn(tempBoardObj4, oppColor).Count != 0)
                            {
                                tempList.Remove(play);
                                continue;
                            }

                            ArrayList temp = new ArrayList();
                            //temp = PreventLoseInOneTurn(tempBoardObj4, playerColor, oppColor);
                            //temp.AddRange(WinInOneTurn(tempBoardObj4, playerColor));
                            //perm.AddRange(temp);
                            //Console.WriteLine("------------------------temp: " + temp.Count);

                            //if (temp.Count == 0)
                            //{
                            //    continue;
                            //}

                            //Console.WriteLine("---------tempList: " + tempList.Count);


                            PreventLoseInNTurnHelper(tempBoardObj4, playerColor, oppColor, numPlays - 1, tempList);
                        }
                        //if (perm.Count != 0)
                        //{
                        //    //Console.WriteLine("Here3");
                        //    tempList.Remove(play);
                        //    continue;
                        //}
                    }
                    numPlays--;
                }
            }
            return tempList;
        }

        /// <summary>
        /// ADDED from the player -- should refactor if this stays here --  changed to static
        /// </summary>
        /// <returns>The next best play strategy.</returns>
        public static ArrayList GetNextBestPlayStrategy(Board board, string playerColor, string oppColor)
        {
            //ArrayList temp = new ArrayList();
            //// player can't win and opp can't win in one move
            //if (Strategy.WinInOneTurn(board, playerColor).Count == 0 && Strategy.WinInOneTurn(board, oppColor).Count == 0)
            //{
            //    return Strategy.DefaultPlay(board, playerColor, oppColor);
            //}

            //// player can win in one move
            ////if (Strategy.WinInOneTurn(board, playerColor).Count != 0)
            ////return Strategy.WinInOneTurn(board, playerColor);
            //temp.AddRange(WinInOneTurn(board, playerColor));
            //JSONEncoder.DumpJson("WinInOneTurn: " + temp);

            //// player can prevent the opp from winning in one move
            ////if (Strategy.PreventLoseInOneTurn(board, playerColor, oppColor).Count != 0)
            ////return Strategy.PreventLoseInOneTurn(board, playerColor, oppColor);
            //if (temp.Count == 0)
            //{
            //    temp.AddRange(PreventLoseInOneTurn(board, playerColor, oppColor));
            //    JSONEncoder.DumpJson("PreventLoseInOneTurn: " + temp);
            //}

            //return temp;

            ArrayList temp = new ArrayList() { };

            // player can win in one move
            temp.AddRange(WinInOneTurn(board, playerColor));

            // include default iff there is no chacne of losing 
            if (temp.Count != 0)
            {
                temp.AddRange(DefaultPlay(board, playerColor, oppColor));
                return temp;
            }

            // player can prevent lose in one move
            /// could be empty for two reasons
            /// 1. there is no play for the player to prevent from losing
            ///     - 
            /// 2. there is no play that lets the opp from winning --> should return all the valid moves (Default case)
            ///     - 
            temp.AddRange(PreventLoseInOneTurn(board, playerColor, oppColor));

            // if temp is empty and default flag is on
            // temp = DefaultPlay
            // this is when whatever the player do the opponent cannot win in the next move
            if (temp.Count == 0)
            {
                temp.AddRange(DefaultPlay(board, playerColor, oppColor));
                return temp;
            }

            // else return the valid plays of the player that prevents the opp from winning the next move
            return temp;
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
            // true when opp cannot win in its next play
            // other, false
            defaultCase = false;

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
            else
            {
                // true when opp cannot win in its next play
                defaultCase = true;
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
        public static ArrayList DefaultPlay(Board board, string playerColor, string oppColor)
        {
            ArrayList validMoves = new ArrayList() { };
            List<string> targets = Player.GetPlayerWorkers(board, playerColor); // { "myColor1", "myColor2" }
            foreach (var workerName in targets)
            {
                foreach (string moveDir in board.directions.Keys)
                {
                    foreach (string buildDir in board.directions.Keys)
                    {
                        // creats a clean copy of the board

                        if (!RuleChecker.IsValidMove(board, workerName, new string[] { moveDir, buildDir }))
                        {
                            continue;
                        }
                        Board tempBoardObj = new Board(board.DumpBoard());
                        tempBoardObj.Move(workerName, moveDir);
                        Board tempBoardObj2 = new Board(tempBoardObj.DumpBoard());

                        List<int> pos = tempBoardObj.PlayerPosition[workerName];
                        if (!RuleChecker.IsValidBuild(tempBoardObj2, workerName, new string[] { moveDir, buildDir }))
                        {
                            continue;
                        }
                        tempBoardObj2.Build(workerName, buildDir);

                        // get rid of case when the playerWorker makes a move that will make the oppWorker win in its next move
                        //if (PreventLoseInOneTurn(tempBoardObj2, playerColor, oppColor).Count == 0)

                        // when the opp cannot win in the current board however can win if I make a play -- need to exclude
                        if (WinInOneTurn(tempBoardObj2, oppColor).Count == 0) // defaultCase is true when opp cannot win in its next play
                        {
                            //Console.WriteLine("added");
                            validMoves.Add(new ArrayList { workerName, new ArrayList { moveDir, buildDir } });
                        }


                    }
                }
            }
            return validMoves;
        }
    }
}
