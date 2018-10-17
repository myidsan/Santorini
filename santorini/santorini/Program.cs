using System;
using System.Collections.Generic;

namespace santorini
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            JSONEncoder parser = new JSONEncoder();
            Board newBoard = new Board();

            newBoard.PrintBoard();
            newBoard.PlaceWorker("white1", 0, 0);
            newBoard.PlaceWorker("blue1", 0, 1);
            newBoard.PlaceWorker("white2", 2, 2);
            newBoard.PlaceWorker("blue2", 3, 4);

            newBoard.Build("white1", "S");
            newBoard.NeighboringCellExists("white1", "W"); // false
            newBoard.NeighboringCellExists("white2", "W"); // True

            //newBoard.Occupied("white2", "NW"); // false
            //newBoard.PrintBoard();
            //newBoard.Move("white2", "W"); // {2,1}
            //newBoard.Move("blue1", "W");
            //newBoard.PrintBoard();
            //newBoard.Move("white1", "N"); // original board
            //newBoard.PrintBoard();
        }
    }
}
