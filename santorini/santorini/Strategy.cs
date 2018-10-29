using System;
using System.Collections.Generic;
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

        public static List<dynamic> WinInOneTurn(Board board, string color)
        {
            // get the coordinate location of the color players
            // check all eight directions of them to see if the height == 3
            // if true, add the move direction to the directions[] and add any build direction
            // append { worker, { move_dir, build_dir } }

            List<string> targets = new List<string>(); // { "White1", White2" }

            foreach (var workerName in board.PlayerPosition.Keys)
            {
                if (workerName.Contains(color))
                {
                    targets.Add(workerName);
                }
            }

            List<dynamic> OneTurnWinPlay = new List<dynamic>();
            foreach (var workerName in targets)
            {
                foreach (var moveDir in board.directions.Keys)
                {
                    if (board.NeighboringCellExists(workerName, moveDir))
                    {
                        List<int> AfterMoveCoordinate = board.GetDesiredPosition(workerName, moveDir);
                        if (board.Board_[AfterMoveCoordinate[0], AfterMoveCoordinate[1]].Height == 3)
                        {
                            // winning scenario
                            foreach (var buildDir in board.directions.Keys)
                            {
                                OneTurnWinPlay.Add(new List<dynamic> { workerName, new List<dynamic> { moveDir, buildDir } });
                            }
                        }
                    }
                }
            }
            return OneTurnWinPlay;
        }

        public static List<dynamic> PreventLoseInOneTurn(Board board, string oppColor)
        {
            return new List<dynamic>();
        }
    }
}
