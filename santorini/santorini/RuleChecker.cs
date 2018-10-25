using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class RuleChecker
    {
        static public bool IsBoardValid()
        {
            string[] allowedPlayers = { "white1", "white2", "blue1", "blue2" };

            if (Board.PlayerPosition.Count != 4) 
            {
                Console.WriteLine("Board is invalid: number of player is {0}", Board.PlayerPosition.Count);
                return false;
            }

            foreach (var player in allowedPlayers)
            {
                if (!Board.PlayerPosition.ContainsKey(player))
                {
                    Console.WriteLine("Board is invalid: doesnt contain {0}", player);
                    return false;
                }
            }
            return true;
        }

        static public bool IsCellValid()
        {
            return true;
        }

        public static bool IsValidMove(string worker, string[] direction)
        {
            // if NeighborCell does not exists
            if (!Board.NeighboringCellExistsHelper(worker, direction[0])) return false;

            // if NeighborCell in given direction is not occupied
            if (Board.OccupiedHelper(worker, direction[0])) return false;

            // if NeighborCell's height is is leq to CurrentCell's height
            if (!Board.IsValidVerticalMove(worker, direction[0])) return false;

            // moving direction is undefined
            if (!Board.directions.ContainsKey(direction[0])) return false;

            return true;
        }

        public static bool IsValidBuild(string worker, string[] direction)
        {
            // if NeighborCell does not exists
            if (!Board.NeighboringCellExistsHelper(worker, direction[1])) return false;

            // if NeighborCell is not occupied to build
            if (Board.OccupiedHelper(worker, direction[1])) return false;

            // if NeighborCell height is less than 4
            if (Board.GetHeight(worker, direction[1]) >= 4) return false;

            return true;
        }
    }
}
