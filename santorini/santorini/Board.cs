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
                new Row()
            };

        // mock-data
        Dictionary<string, List<int>> playerPosition = new Dictionary<string, List<int>>();

        Dictionary<string, List<int>> directions = new Dictionary<string, List<int>>(){
            {"N", new List<int> {-1, 0} },
            {"S", new List<int> {1, 0} },
            {"W", new List<int> {0, -1} },
            {"E", new List<int> {0, 1} },
            {"NW", new List<int> {-1, -1} },
            {"NE", new List<int> {-1, 1} },
            {"SW", new List<int> {1, -1} },
            {"SE", new List<int> {1, 1} }
        };

        public void PrintBoard()
        {
            foreach (var row in board)
            {
                row.PrintRow();
            }
        }

        // queries
        public bool NeighboringCellExists(String worker, String direction)
        {
            if (playerPosition.ContainsKey(worker))
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
            }
            else
            {
                throw new Exception(worker + " was not found");
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

        // commands
        public Board Move(String worker, String direction)
        {
            try
            {

                List<int> finalPosition = GetWorkerDesiredPosition(worker, direction);

                // init cell position of the worker
                Row initRowPos = board[playerPosition[worker][0]];
                Cell initCellPos = initRowPos.row[playerPosition[worker][1]];
                initCellPos.Worker = null;


                // modify worker position
                playerPosition[worker] = finalPosition;
                Row rowPos = board[finalPosition[0]];
                Cell cellPos = rowPos.row[finalPosition[1]];
                cellPos.Worker = worker;
            }
            catch (Exception)
            {
                throw new Exception("can't move there");
            }

            return this;
        }

        // helper command
        List<int> GetWorkerDesiredPosition(String worker, string direction)
        {
            // returns a valid coordinate that the worker can go 
            // in List<int> format

            // get current coordinate of the player {row, cell}
            List<int> workerPosition = playerPosition[worker];
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

            return finalPosition;
        }

        // helper command
        public void PlacePlayer(String Worker, int row, int col)
        {
            var rowPos = board[row];
            var cellPos = rowPos.row[col];
            if (cellPos.Worker != null)
            {
                Console.WriteLine("occupied by other worker");
                return;
            }
            cellPos.Worker = Worker;
            // update dictionary
            playerPosition[Worker] = new List<int> {row, col};

            // print dictionary for testing purpose
            foreach (KeyValuePair<string, List<int>> kv in playerPosition)
            {
                Console.WriteLine(kv.Key);
                kv.Value.ForEach(Console.WriteLine);
            }

            return;
        }

       
    }








    public class Row
    {
        public IList<Cell> row = new List<Cell>() {
                new Cell(),
                new Cell(),
                new Cell(),
                new Cell(),
                new Cell()
            };

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
