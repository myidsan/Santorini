using System;
namespace santorini.designs
{
    public class designRefereee
    {
        /// <summary>
        /// takes two player instances to create a referee instance
        /// </summary>
        /// Contract:
        /// 1. The referee should keep an internal board object itself without getting it 
        /// from the player to avoid any invalid board inputs
        /// 2. 
        public designRefereee(Player a, Player b) {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns> color of the player </returns>
        public string Name() { return ""; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> placement reflected board of the given player</returns>
        public Cell[,] Placement() { return new Cell[5,5]; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> play reflected board of the given player </returns>
        public Cell[,] Direction() { return new Cell[5, 5]; }
    }
}
