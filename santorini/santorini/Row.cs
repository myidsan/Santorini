using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Row
    {
        public IList<Cell> row = new List<Cell>();

        public Row() {}

        public Row(JToken rowArray, int rowIndex)
        {
            int colIndex = 0;
            foreach (var cell in rowArray)
            {
                var height = 0;
                var worker = "";
                if (cell.GetType() == typeof(JArray))
                {
                    height = (int)cell[0];
                    worker = (string)cell[1];
                    if (worker != null)
                    {
                        Board.PlayerPosition[worker] = new List<int>() { rowIndex, colIndex };
                    }
                }
                else
                {
                    height = (int)cell;
                    worker = null;
                }
                Cell newCell = new Cell(height, worker);
                row.Add(newCell);
                colIndex++;
            }
        }



        public void PrintRow()
        {
            string row_string = "";
            foreach (var cell in row)
            {
                row_string += (cell.Height, cell.Worker) + " ";
            }
            Console.WriteLine(row_string);
        }
    }

    public class Cell
    {
        private int height = 0;
        private string worker = null;

        public int Height { get => height; set => height = value; }
        public string Worker { get => worker; set => worker = value; }

        public Cell() { }

        public Cell(int pHeight, string pWorker)
        {
            Height = pHeight;
            Worker = pWorker;
        }
    }
}
