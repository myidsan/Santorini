using System;
using System.Collections.Generic;

namespace santorini
{
    public class Board
    {
        IList<Row> board = new List<Row>() {
                new Row(),
                new Row(),
                new Row(),
                new Row(),
                new Row(),
            };

        // mock-data
        Dictionary<string, List<int>> playerPosition = new Dictionary<string, List<int>>(){
            {"white1", new List<int> {0, 0} },
            {"white2", new List<int> {2, 2} },
            {"blue1", new List<int> {2, 3} },
            {"blue2", new List<int> {0, 4} },
        };

        Dictionary<string, List<int>> directions = new Dictionary<string, List<int>>(){
            {"N", new List<int> {1, 0} },
            {"S", new List<int> {-1, 0} },
            {"W", new List<int> {0, -1} },
            {"E", new List<int> {0, 1} },
            {"NW", new List<int> {1, -1} },
            {"NE", new List<int> {1, 1} },
            {"SW", new List<int> {-1, -1} },
            {"SE", new List<int> {-1, 1} }
        };

        public void PrintBoard()
        {
            foreach (var row in board)
            {
                row.PrintRow();
            }
        }

        public bool NeighboringCellExists(String worker, String direction)
        {
            List<int> workerPosition = playerPosition[worker]; // {row, cell}

            // parse the direction
            List<int> desiredDirection = directions[direction];
            List<int> finalPosition = new List<int>();

            for (int i = 0; i < workerPosition.Count; i++)
            {  
                finalPosition.Add(workerPosition[i] + desiredDirection[i]);
                if (finalPosition[i] < 0 || finalPosition[i] > board.Count)
                {
                    Console.WriteLine("false");
                    return false;
                }
            }
            return true;
        }

        public bool Occupied(String worker, String direction)
        {
            List<int> workerPosition = playerPosition[worker]; // {row, cell}

            // parse the direction
            List<int> desiredDirection = directions[direction];
            List<int> finalPosition = new List<int>();

            if (NeighboringCellExists(worker, direction))
            {
                for (int i = 0; i < workerPosition.Count; i++)
                {
                    finalPosition.Add(workerPosition[i] + desiredDirection[i]);
                }
            }
            //Console.WriteLine(finalPosition[0]);
            //Console.WriteLine(finalPosition[1]);

            var rowPos = board[finalPosition[0]];
            Console.WriteLine(rowPos.row);
            var cellPos = rowPos.row[finalPosition[1]];
            Console.WriteLine(cellPos.Worker);

            Console.WriteLine(cellPos.Worker);
            if (cellPos.Worker != null)
            {
                Console.WriteLine("occupied go somewhere else");
                return true;
            }
            return false;
        }

        public int GetHeight(String worker, String direction)
        {
            List<int> workerPosition = playerPosition[worker]; // {row, cell}

            // parse the direction
            List<int> desiredDirection = directions[direction];
            List<int> finalPosition = new List<int>();

            if (NeighboringCellExists(worker, direction))
            {
                for (int i = 0; i < workerPosition.Count; i++)
                {
                    finalPosition.Add(workerPosition[i] + desiredDirection[i]);
                }
            }

            var rowPos = board[finalPosition[0]];
            var cellPos = rowPos.row[finalPosition[1]];

            Console.WriteLine(cellPos.Height);
            return cellPos.Height;
        }
    }








    public class Row
    {
        public IList<Cell> row = new List<Cell>() {
                new Cell(),
                new Cell(),
                new Cell(2, "white2"),
                new Cell(3, "blue1"),
                new Cell()
            };

        public void PrintRow()
        {
            string row_string = "";
            foreach (var cell in row)
            {
                row_string += cell.Height.ToString() + ' ';
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
