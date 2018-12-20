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

        // override method for testing purpose
        public static ArrayList PreventLoseInNTurn(Board board, string playerColor, string oppColor, int numPlayz)
        {
            ArrayList resultList = new ArrayList() { };

            // get init possible plays of the player
            ArrayList masterList = new ArrayList() { };
            masterList.AddRange(GetNextBestPlayStrategy(board, playerColor, oppColor));

            if (numPlayz == 1)
            {
                return masterList;
            }

            //// debug
            //ArrayList temp = new ArrayList() { };
            //temp.Add(new ArrayList { "Blue1", new ArrayList { "S", "N" } });
            //masterList = temp;

            // DFS on each play
            foreach (ArrayList play in masterList)
            {
                string worker = (string)play[0];
                string[] directions = ((IEnumerable)play[1]).Cast<object>()
                                                             .Select(x => x.ToString())
                                                             .ToArray();
                if (directions.Length == 1)
                {
                    resultList.Add(play);
                    continue;
                }

                Board newBoard = new Board(board.DumpBoard());
                newBoard.Move(worker, directions[0]);
                newBoard.Build(worker, directions[1]);

                List<Board> oppBoards = GetAllBoards(newBoard, oppColor);

                bool completeAlive = true;

                foreach (Board oBoard in oppBoards)
                {
                    if (!completeAlive) break;

                    if (IsWinner(oBoard, oppColor))
                    {
                        completeAlive = false;
                        break;
                    }

                    completeAlive = completeAlive && AliveMoveAfterNTurn(oBoard, playerColor, oppColor, numPlayz - 1);
                }

                if (completeAlive)
                {
                    resultList.Add(play);
                }
            }

            return resultList;
        }

        public static ArrayList PreventLoseInNTurn(Board board, string playerColor, string oppColor)
        {
            int numPlays = Utility.ReadLookAhead();

            ArrayList resultList = new ArrayList() { };

            // get init possible plays of the player
            ArrayList masterList = new ArrayList() { };
            masterList.AddRange(GetNextBestPlayStrategy(board, playerColor, oppColor));

            if (numPlays == 1)
            {
                return masterList;
            }
            // DFS on each play
            foreach (ArrayList play in masterList)
            {
                string worker = (string)play[0];
                string[] directions = ((IEnumerable)play[1]).Cast<object>()
                                                             .Select(x => x.ToString())
                                                             .ToArray();
                if (directions.Length == 1)
                {
                    resultList.Add(play);
                    continue;
                }

                Board newBoard = new Board(board.DumpBoard());
                newBoard.Move(worker, directions[0]);
                newBoard.Build(worker, directions[1]);

                List<Board> oppBoards = GetAllBoards(newBoard, oppColor);

                bool completeAlive = true;

                foreach (Board oBoard in oppBoards)
                {
                    if (!completeAlive) break;

                    if (IsWinner(oBoard, oppColor))
                    {
                        completeAlive = false;
                        break;
                    }

                    completeAlive = completeAlive && AliveMoveAfterNTurn(oBoard, playerColor, oppColor, numPlays - 1);
                }

                if (completeAlive)
                {
                    resultList.Add(play);
                }
            }

            return resultList;
        }

        public static bool AliveMoveAfterNTurn(Board board, string playerColor, string oppColor, int numPlay)
        {
            if (numPlay == 0) return true;

            if (IsWinner(board, oppColor))
            {
                return false;
            }

            // get smart plays n = 1
            // ArrayList nextPlays = GetNextBestPlayStrategy(board, playerColor, oppColor); //this is after both plays once
            // get all dumb plays as a whole
            ArrayList nextPlays = GetAllPossibleplays(board, playerColor);
            //Console.WriteLine(JSONEncoder.DumpJson(board.DumpBoard()));
            //Console.WriteLine(numPlay + ": " + playerColor + ": " + nextPlays.Count);

            if (nextPlays.Count == 0) return false;

            bool innerCompleteAlive = true;

            foreach (ArrayList nPlay in nextPlays)
            {
                string worker = (string)nPlay[0];
                string[] directions = ((IEnumerable)nPlay[1]).Cast<Object>()
                                                             .Select(x => x.ToString())
                                                             .ToArray();
                // me winning is not important
                // but me winning and opp not being able to win in that turn counts
                if (directions.Length == 1 && WinInOneTurn(board, oppColor).Count == 0)
                {
                    return true;
                }

                if (directions.Length == 1)
                {
                    continue;
                }

                Board newBoard = new Board(board.DumpBoard());
                newBoard.Move(worker, directions[0]);
                if (board.Equals(newBoard))
                {
                    Console.WriteLine("move is wrong");
                    return false;
                }
                newBoard.Build(worker, directions[1]);
                if (board.Equals(newBoard))
                {
                    Console.WriteLine("build is wrong");
                    return false;
                }

                bool innerPartialAlive = true;

                if (WinInOneTurn(newBoard, oppColor).Count != 0)
                {
                    innerPartialAlive = false;
                    innerCompleteAlive = innerCompleteAlive && innerPartialAlive;
                    if (!innerCompleteAlive) break;
                }
                else
                {
                    List<Board> nextBoards = GetAllBoards(newBoard, oppColor);
                    if (nextBoards.Count == 0)
                    {
                        innerPartialAlive = false;
                        break;
                    }
                    foreach (Board nBoard in nextBoards)
                    {
                        if (!innerPartialAlive) 
                        {
                            innerCompleteAlive = innerCompleteAlive && innerPartialAlive;
                            if (!innerCompleteAlive) break;
                        }
                        if (IsWinner(nBoard, oppColor))
                        {
                            innerPartialAlive = false;
                            innerCompleteAlive = innerCompleteAlive && innerPartialAlive;
                            if (!innerCompleteAlive) break;
                        }
                        else
                        {
                            innerPartialAlive = innerPartialAlive && AliveMoveAfterNTurn(nBoard, playerColor, oppColor, numPlay - 1);
                        }
                    }
                }
                innerCompleteAlive = innerCompleteAlive && innerPartialAlive;
                if (!innerCompleteAlive)
                    break;
            }
            return innerCompleteAlive;
        }

        public static bool IsWinner(Board board, string playerColor)
        {
            List<string> targets = Player.GetPlayerWorkers(board, playerColor); // { "myColor1", "myColor2" }
            foreach (var player in targets)
            {
                List<int> playerPos = board.PlayerPosition[player];
                if (board.Board_[playerPos[0], playerPos[1]].Height == 3)
                {
                    return true;
                }
            }
            return false;
        }

        // gets all possible boards from the player/opp in respective turn for its best plays 
        public static List<Board> GetAllBoards(Board board, string playerColor)
        {
            List<Board> possibleBoards = new List<Board>() { };

            ArrayList possiblePlays = GetAllPossibleplays(board, playerColor);
            foreach (ArrayList play in possiblePlays)
            {
                // could be wrapped in a single play function
                string worker = (string)play[0];
                string[] directions = ((IEnumerable)play[1]).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();

                Board tempBoardObj1 = new Board(board.DumpBoard());

                if (directions.Length == 1)
                {
                    tempBoardObj1.Move(worker, directions[0]);
                    possibleBoards.Add(tempBoardObj1);
                }
                else
                {
                    tempBoardObj1.Move(worker, directions[0]);
                    tempBoardObj1.Build(worker, directions[1]);
                    possibleBoards.Add(tempBoardObj1);
                }
            }
            return possibleBoards;
        }

        public static List<Board> GetAllAliveBoards(Board board, string playerColor, string oppColor)
        {
            ArrayList possiblePlays = GetNextBestPlayStrategy(board, playerColor, oppColor);
            List<Board> possibleBoards = new List<Board>() { };

            foreach (ArrayList play in possiblePlays)
            {
                // could be wrapped in a single play function
                string worker = (string)play[0];
                string[] directions = ((IEnumerable)play[1]).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();

                Board tempBoardObj1 = new Board(board.DumpBoard());
                tempBoardObj1.Move(worker, directions[0]);
                tempBoardObj1.Build(worker, directions[1]);

                if (WinInOneTurn(tempBoardObj1, oppColor).Count == 0)
                {
                    possibleBoards.Add(tempBoardObj1);
                }
            }
            return possibleBoards;
        }

        /// <summary>
        /// ADDED from the player -- should refactor if this stays here --  changed to static
        /// </summary>
        /// <returns>The next best play strategy.</returns>
        public static ArrayList GetNextBestPlayStrategy(Board board, string playerColor, string oppColor)
        {
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

        public static ArrayList GetAllPossibleplays(Board board, string playerColor)
        {
            ArrayList validMoves = new ArrayList() { };
            List<string> targets = Player.GetPlayerWorkers(board, playerColor); // { "myColor1", "myColor2" }
            foreach (var workerName in targets)
            {
                foreach (string moveDir in board.directions.Keys)
                {
                    if (RuleChecker.IsValidMove(board, workerName, new string[] { moveDir }))
                    {
                        validMoves.Add(new ArrayList { workerName, new ArrayList { moveDir } });
                    }
                    foreach (string buildDir in board.directions.Keys)
                    {
                        if (!RuleChecker.IsValidMove(board, workerName, new string[] { moveDir, buildDir })) continue;
                        Board tempBoardObj1 = new Board(board.DumpBoard());
                        tempBoardObj1.Move(workerName, moveDir);
                        if (!RuleChecker.IsValidBuild(tempBoardObj1, workerName, new string[] { moveDir, buildDir })) continue;
                        tempBoardObj1.Build(workerName, buildDir);

                        List<int> finalPos = tempBoardObj1.PlayerPosition[workerName];
                        if (!tempBoardObj1.Equals(board) && tempBoardObj1.Board_[finalPos[0], finalPos[1]].Height == 3)
                        {
                            validMoves.Add(new ArrayList { workerName, new ArrayList { moveDir } });
                        }
                        Board tempBoardObj2 = new Board(tempBoardObj1.DumpBoard());
                        tempBoardObj2.Build(workerName, moveDir);
                        if (!tempBoardObj2.Equals(tempBoardObj1))
                        {
                            validMoves.Add(new ArrayList { workerName, new ArrayList { moveDir, buildDir } });
                        }
                    }
                }
            }
            return validMoves;
        }
    }
}
