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
        IList<Row> board = new List<Row>();
        JSONEncoder encoder = new JSONEncoder();
        public Board(JArray boardArray)
        {
            int rowIndex = 0;
            foreach (var item in boardArray)
            {
                Row newRow = new Row(item, rowIndex);
                board.Add(newRow);
                rowIndex++;
            }
        }

        static Dictionary<string, List<int>> playerPosition = new Dictionary<string, List<int>>();
        static public Dictionary<string, List<int>> PlayerPosition { get => playerPosition; set => playerPosition = value; }

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

        /// queries
        public bool NeighboringCellExists(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction))
            {
                Console.WriteLine(true);
                return true;
            }
            Console.WriteLine(false);
            return false;
        }

        public bool NeighboringCellExistsHelper(string worker, string direction)
        {
            if (playerPosition.ContainsKey(worker))
            {
                List<int> desiredDirection = directions[direction];
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                for (int i = 0; i < finalPosition.Count; i++)
                {
                    if (finalPosition[i] < 0 || finalPosition[i] > board.Count - 1)
                    {
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

        public bool Occupied(string worker, string direction)
        {
            if (OccupiedHelper(worker, direction))
            {
                Console.WriteLine(true);
                return true;
            }
            Console.WriteLine(false);
            return false;
        }

        public bool OccupiedHelper(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction))
            {
                List<int> workerPosition = playerPosition[worker]; // {row, cell}
                List<int> finalPosition = GetDesiredPosition(worker, direction);

                var rowPos = board[finalPosition[0]];
                var cellPos = rowPos.row[finalPosition[1]];

                if (cellPos.Worker != null)
                {
                    return true;
                }
                return false;
            }
            throw new Exception("Neighboring cell doesn't exists so it can't be occupied");
        }

        public int GetHeight(string worker, string direction)
        {
           
            if (NeighboringCellExistsHelper(worker, direction))
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

        /// commands
        public Board Move(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction) && !OccupiedHelper(worker,direction))
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
            this.PrintBoard();
            return this;
        }

        public Board Build(string worker, string direction)
        {
            if (NeighboringCellExistsHelper(worker, direction) && !OccupiedHelper(worker, direction))
            {
                List<int> desiredPosition = GetDesiredPosition(worker, direction);
                Row rowPos = board[desiredPosition[0]];
                Cell cellPos = rowPos.row[desiredPosition[1]];
                if (cellPos.Height < 4)
                {
                    cellPos.Height++;
                }
            }
            this.PrintBoard();
            return this;
        }

        /// helper command starts
        List<int> GetDesiredPosition(string worker, string direction)
        {
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

        public void PlaceWorker(string Worker, int row, int col)
        {

            var rowPos = board[0];
            var cellPos = rowPos.row[col];
            if (cellPos.Worker != null)
            {
                Console.WriteLine("occupied by other worker");
                return;
            }
            cellPos.Worker = Worker;
            // update dictionary
            playerPosition[Worker] = new List<int> {row, col};

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
        
        public void PrintBoard()
        {
            JArray result = new JArray();
            foreach (var row in board)
            {
                JArray rowArr = row.PrintRow();
                result.Add(rowArr);

            }
            string JSONresult = JsonConvert.SerializeObject(result);
            Console.WriteLine(JSONresult);
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
