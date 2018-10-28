using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Board
    {
        private Cell[,] board;
        public Cell[,] Board_ { get => board;}

        static Dictionary<string, List<int>> playerPosition;
        static public Dictionary<string, List<int>> PlayerPosition { get => playerPosition; set => playerPosition = value; }

        public Board(JArray boardArray)
        {
            board = new Cell[5, 5];
            PlayerPosition = new Dictionary<string, List<int>>();

            int rowLength = board.GetLength(0);
            int colLength = board.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    board[i, j] = new Cell(boardArray[i][j], i ,j);
                }
            }
        }

        // to handle temporary board during parsing player move/build
        public Board(Cell[,] tempBoard)
        {
            board = tempBoard;
        }

        static public Dictionary<string, List<int>> directions = new Dictionary<string, List<int>>(){
            {"N", new List<int> {-1, 0} },
            {"S", new List<int> {1, 0} },
            {"W", new List<int> {0, -1} },
            {"E", new List<int> {0, 1} },
            {"NW", new List<int> {-1, -1} },
            {"NE", new List<int> {-1, 1} },
            {"SW", new List<int> {1, -1} },
            {"SE", new List<int> {1, 1} }
        };

        /// queries
        public bool NeighboringCellExists(string worker, string direction)
        {
            bool result = NeighboringCellExistsHelper(worker, direction);
            Console.WriteLine(result);
            return result;
        }

        public bool NeighboringCellExistsHelper(string worker, string direction)
        {
            if (PlayerPosition.ContainsKey(worker) && directions.ContainsKey(direction))
            {
                List<int> desiredDirection = directions[direction];
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                for (int i = 0; i < finalPosition.Count; i++)
                {
                    if (finalPosition[i] < 0 || finalPosition[i] > board.GetLength(0) - 1)
                    {
                        return false;
                    }
                }
            }
            else
            {
                throw new Exception("NeighboringCellExistsHelper: Invalid input parameter");
            }
            return true;
        }

        public bool Occupied(string worker, string direction)
        {
            bool result = OccupiedHelper(worker, direction);
            Console.WriteLine(result);
            return result;
        }

        public bool OccupiedHelper(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction))
            {
                List<int> workerPosition = PlayerPosition[worker]; // {row, cell}
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                var cellPos = board[finalPosition[0], finalPosition[1]];

                if (cellPos.Worker != null)
                {
                    return true;
                }
                return false;
            }
            throw new IndexOutOfRangeException("Neighboring cell doesn't exists so it can't be occupied");
        }

        public int GetHeight(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction))
            {
                List<int> workerPosition = playerPosition[worker]; // {row, cell}
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                var cellPos = board[finalPosition[0], finalPosition[1]];
                //Console.WriteLine(cellPos.Height);
                return cellPos.Height;
            }
            throw new IndexOutOfRangeException("Neighboring cell doesn't exists so it can't have height");
        }

        public int GetHeight(string worker)
        {
            List<int> workerPosition = playerPosition[worker];
            var cellPos = board[workerPosition[0], workerPosition[1]];
            return cellPos.Height;
        }

        public Cell[,] ExecutePlay(string worker, string[] direction)
        {
            // executes the play in order and 

            return board;
        }

        /// commands
        public Cell[,] Move(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction) && !OccupiedHelper(worker,direction))
            {
                // init cell position of the worker
                List<int> initialPosition = playerPosition[worker];
                board[initialPosition[0], initialPosition[1]].Worker = null;

                // modify worker position
                List<int> finalPosition = GetDesiredPosition(worker, direction);
                playerPosition[worker] = finalPosition;
                board[finalPosition[0], finalPosition[1]].Worker = worker;
            }
            //PrintBoard();
            return board;
        }

        public Cell[,] Build(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction) && !OccupiedHelper(worker, direction))
            {
                List<int> desiredPosition = GetDesiredPosition(worker, direction);
                Cell cellPos = board[desiredPosition[0], desiredPosition[1]];
                if (cellPos.Height < 4)
                {
                    cellPos.Height++;
                }
            }
            //PrintBoard();
            return board;
        }

        /// helper command starts
        public List<int> GetDesiredPosition(string worker, string direction)
        {
            // get current coordinate of the player {row, cell}
            List<int> workerPosition = PlayerPosition[worker];
            // parse the direction
            List<int> desiredDirection = directions[direction];
            List<int> finalPosition = new List<int>();

            for (int i = 0; i < workerPosition.Count; i++)
            {
                finalPosition.Add(workerPosition[i] + desiredDirection[i]);
            }
            return finalPosition;
        }

        public void PrintBoard()
        {
            List<List<dynamic>> result = new List<List<dynamic>>();
            for (int i = 0; i < Board_.GetLength(0); i++)
            {
                List<dynamic> row = new List<dynamic>();
                for (int j = 0; j < Board_.GetLength(1); j++)
                {
                    row.Add(Board_[i, j].PrintCell());
                }
                result.Add(row);
            }
            string JSONresult = JsonConvert.SerializeObject(result);
            Console.WriteLine((JValue)JSONresult);
        }

        public void PlaceWorker(string Worker, int row, int col)
        {
            var cellPos = Board_[row, col];

            if (cellPos.Worker != null)
            {
                Console.WriteLine("occupied by other worker");
                return;
            }
            cellPos.Worker = Worker;
            // update dictionary
            playerPosition[Worker] = new List<int> { row, col };

            return;
        }
        public void PrintPlayerPosition(Dictionary<string, List<int>> playerPosition)
        {
            foreach (KeyValuePair<string, List<int>> kv in playerPosition)
            {
                Console.WriteLine(kv.Key);
                kv.Value.ForEach(Console.WriteLine);
            }
        }
        /// helper command ends


        /// Driver
        public void RunCommand(JArray action)
        {
            List<string> argsList = new List<string>();
            string methodName = action[0].ToString();

            // need to parse the command into C# name style
            string command = ParseMethodName(methodName);
           
            for (int i = 1; i < action.Count; i++)
            {
                argsList.Add(action[i].ToString());
            }
            string[] args = argsList.ToArray();

            MethodInfo mi = this.GetType().GetMethod(command);
            // null - no param for the method call
            // or pass in array of paramters
            if (args.Length == 0)
            {
                args = null;
            }
            var result = mi.Invoke(this, args);
        }

        public string ParseMethodName(string name)
        {
            string[] parsing = name.Split('?');
            string[] parsingTwo = parsing[0].Split('-');
            string command = "";
            foreach (var item in parsingTwo)
            {
                command += item.Substring(0, 1).ToUpper() + item.Substring(1);
            }
            return command;
        }
    }
}
