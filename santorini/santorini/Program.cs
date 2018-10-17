using System;
using System.Collections.Generic;

namespace santorini
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Board newBoard = new Board();

            newBoard.PrintBoard();
            newBoard.PlacePlayer("white1", 0, 0);
            newBoard.NeighboringCellExists("white1", "W");
            newBoard.PlacePlayer("white2", 2, 2);
            newBoard.NeighboringCellExists("white2", "W");
            newBoard.PrintBoard();
            newBoard.Move("white2", "N");
            newBoard.PrintBoard();
            newBoard.PlacePlayer("blue1", 1, 3);
            newBoard.Move("blue1", "W");
            //newBoard.Occupied("blue1", "W"); // True
            newBoard.PrintBoard();
            //newBoard.GetHeight("blue1", "W"); //2
        }
    }
}
