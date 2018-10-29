using System;
using System.Collections.Generic;

namespace santorini
{
    public class Player
    {
        //public static List<String> playerWorkers;
        //public static List<String> OppWorkers;
        string color = "";
        string oppColor = "";
        public string Color { get => color;}
        public string OppColor { get => oppColor; }

        public Player()
        {
            color = RegisterPlayer();
            oppColor = GetOpponentColor();
        }

        public string GetOpponentColor()
        {
            foreach (var pColor in workerColors)
            {
                if (pColor != color)
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
            int i = rand.Next(0, 1);
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
    }
}
