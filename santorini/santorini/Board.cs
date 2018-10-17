using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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

        // queries
        public bool NeighboringCellExists(String worker, String direction)
        {
            if (playerPosition.ContainsKey(worker))
            {
                List<int> desiredDirection = directions[direction];
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                for (int i = 0; i < finalPosition.Count; i++)
                {
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
            if (NeighboringCellExists(worker, direction))
            {
                List<int> workerPosition = playerPosition[worker]; // {row, cell}
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                var rowPos = board[finalPosition[0]];
                var cellPos = rowPos.row[finalPosition[1]];

                if (cellPos.Worker != null)
                {
                    Console.WriteLine("occupied go somewhere else");
                    return true;
                }
                Console.WriteLine("not occupied");
                return false;
            }
            throw new Exception("Neighboring cell doesn't exists so it can't be occupied");
        }

        public int GetHeight(String worker, String direction)
        {
           
            if (NeighboringCellExists(worker, direction))
            {
                List<int> workerPosition = playerPosition[worker]; // {row, cell}
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                var rowPos = board[finalPosition[0]];
                var cellPos = rowPos.row[finalPosition[1]];
                Console.WriteLine(cellPos.Height);
                return cellPos.Height;
            }
            throw new Exception("Neighboring cell doesn't exists so it can't have height");
        }

        // commands
        public Board Move(String worker, String direction)
        {
            if (NeighboringCellExists(worker, direction) && !Occupied(worker,direction))
            {
                // init cell position of the worker
                Row initRowPos = board[playerPosition[worker][0]];
                Cell initCellPos = initRowPos.row[playerPosition[worker][1]];
                initCellPos.Worker = null;

                // modify worker position
                List<int> finalPosition = GetDesiredPosition(worker, direction);
                playerPosition[worker] = finalPosition;
                Row rowPos = board[finalPosition[0]];
                Cell cellPos = rowPos.row[finalPosition[1]];
                cellPos.Worker = worker;
            }
            return this;
        }

        public Board Build(String worker, String direction)
        {
            List<int> desiredPosition = GetDesiredPosition(worker, direction);
            desiredPosition.ForEach(item => Console.WriteLine(item));
            Row rowPos = board[desiredPosition[0]];
            Cell cellPos = rowPos.row[desiredPosition[1]];

            if (NeighboringCellExists(worker, direction) && !Occupied(worker, direction) && cellPos.Height < 4)
            {
                cellPos.Height++;
            }
            else 
            {
                Console.WriteLine("can't build there");
            }
            return this;
        }

        // helper command
        List<int> GetDesiredPosition(String worker, string direction)
        {
            // returns a valid coordinate that the worker can go 
            // in List<int> format

            // get current coordinate of the player {row, cell}
            List<int> workerPosition = playerPosition[worker];
            // parse the direction
            List<int> desiredDirection = directions[direction];
            List<int> finalPosition = new List<int>();

            for (int i = 0; i < workerPosition.Count; i++)
            {
                finalPosition.Add(workerPosition[i] + desiredDirection[i]);
            }

            return finalPosition;
        }

        // helper command
        public void PlaceWorker(String Worker, int row, int col)
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
            //PrintPlayerPosition(playerPosition);
            return;
        }

        // helper command
        public void PrintPlayerPosition(Dictionary<string, List<int>> playerPosition)
        {
            foreach (KeyValuePair<string, List<int>> kv in playerPosition)
            {
                Console.WriteLine(kv.Key);
                kv.Value.ForEach(Console.WriteLine);
            }
        }

        // helper command
        public void PrintBoard()
        {
            foreach (var row in board)
            {
                row.PrintRow();
            }
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
