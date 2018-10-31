using System;
using System.Collections;
using System.Collections.Generic;

namespace santorini
{
    public class Player
    {
        //public static List<String> playerWorkers;
        //public static List<String> OppWorkers;
        string playerColor = "";
        string oppColor = "";
        public string PlayerColor { get => playerColor; }
        public string OppColor { get => oppColor; }

        public Player()
        {
            playerColor = RegisterPlayer();
            oppColor = GetOpponentColor();
        }

        public string GetOpponentColor()
        {
            foreach (var pColor in workerColors)
            {
                if (pColor != playerColor)
                {
                    return pColor;
                }
            }
            return "";
        }

        List<List<int>> InitPlacementCandidates = new List<List<int>>
        {
            new List<int> {0, 0},
            new List<int> {0, 4},
            new List<int> {4, 4},
            new List<int> {4, 0}
        };

        List<string> workerColors = new List<string>
        {
            "Blue", "White"
        };

        public string RegisterPlayer()
        {
            Random rand = new Random();
            int i = rand.Next(0, 2);
            Console.WriteLine(i);
            return workerColors[i];
        }

        public List<List<int>> PlacePlayerWorkers(Board board, string color) 
        {
            int count = 1;
            List<List<int>> coordinates = new List<List<int>>();
            foreach (var candidate in InitPlacementCandidates)
            {
                if (count == 3) break;
                if (RuleChecker.IsValidInitPlacement(board, candidate))
                {
                    string worker = color + Convert.ToString(count);
                    board.PlaceWorker(worker, candidate);
                    count++;
                    coordinates.Add(candidate);
                }
            }
            return coordinates;
        }

        public ArrayList GetNextBestPlay(Board board, string playerColor, string oppColor)
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

        public static List<string> GetPlayerWorkers(Board board, string playerColor)
        {
            List<string> targets = new List<string>();
            foreach (var workerName in board.PlayerPosition.Keys)
            {
                if (workerName.Contains(playerColor))
                {
                    targets.Add(workerName);
                }
            }
            return targets;
        }
    }
}
