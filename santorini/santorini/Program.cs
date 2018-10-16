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
            newBoard.NeighboringCellExists("white1", "W");
            newBoard.NeighboringCellExists("white2", "W");
            newBoard.Occupied("blue1", "W");
            newBoard.GetHeight("blue1", "W"); //2
        }
    }
}
