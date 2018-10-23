using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;

namespace santorini {

    // <summary>
    // This class implements the board of the game.
    // It contains necessary methods to for workers to move 
    // and build on the board.
    // it contains information that the board should be aware of.
    // </summary>
    interface design<T>
    {
        // dictionary_ key: String worker, value: List<int> {row, col}
        //public Dictionary<string, List<int>> playerPosition;

        // dictionary_  key: String worker, value: List<int> ex N: {-1, 0}
        //Dictionary<string, List<int>> directions;

        /// <summary>
        /// Checks if the cell in the direction adjacent of the worker exists
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary
        /// </summary>
        /// <returns>
        /// A boolean whether a cell adjacent to the worker 
        /// exists in the given direction
        /// </returns>
        bool NeighboringCellExists(String worker, String direction);

        /// <summary>
        /// Checks if the cell in the direction adjacent of the worker is occupied by another worker
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        ///           NeighboringCellExists --> true
        /// </summary>
        /// <returns>
        /// A boolean whether a cell adjacent to the worker 
        /// is occupied by another worker
        /// </returns>
        bool Occupied(String worker, String direction);

        /// <summary>
        /// Increase the height of the cell by one
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        ///           NeighboringCellExists --> true
        /// </summary>
        /// <returns>
        /// An integer representing the height of the cell
        /// </returns>
        int GetHeight(String worker, String direction);

        /// <summary>
        /// Move the given worker in the given direction
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        ///           NeighboringCellExists --> true,
        ///           Occupied --> false
        /// </summary>
        /// <returns>
        /// The state of the board
        /// </returns>
        Cell[,] Move(String worker, String direction);

        /// <summary>
        /// Increment the height of the adjacent cell of the worker in the given direction
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        ///           NeighboringCellExists --> true,
        ///           Occupied --> false
        /// </summary>
        /// <returns>
        /// The state of the board
        /// </returns>
        Cell[,] Build(String worker, String direction);


        //// helpers

        /// <summary>
        /// Calculates the desired position of the worker in relation to the given direction
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        /// </summary>
        /// <returns>
        /// A list of integers in {row, col} format
        /// </returns>
        List<int> GetDesiredPosition(String worker, string direction);

        /// <summary>
        /// places a worker in the given grid coordinate
        /// contract: worker should be in the playerPosition dictionary, 
        ///           direction should be in the directions dictionary,
        ///           Occupied --> false
        /// </summary>
        /// <returns>
        /// The state of the board
        /// </returns>
        void PlaceWorker(String Worker, int row, int col);

        // Debugging methods
        void PrintPlayerPosition(Dictionary<string, List<int>> playerPosition);
        void PrintBoard();
    }

    /// <summary>
    /// Cells consists a row.
    /// </summary>
    public class Cell
    {
        private int height;
        private string worker;

        public int Height { get => height; set => height = value; }
        public string Worker { get => worker; set => worker = value; }

        public Cell(JToken candidate, int rowIndex, int colIndex)
        {
            if (candidate.GetType() == typeof(JArray))
            {
                this.Height = (int)candidate[0];
                this.worker = (string)candidate[1];
                if (this.worker != null)
                {
                    Board.PlayerPosition.Add(this.worker, new List<int> { rowIndex, colIndex });
                }
            }
            else
            {
                this.Height = (int)candidate;
                this.Worker = null;
            }
        }

        public void PrintCell()
        {
            dynamic[] arr = { this.Height, this.worker };

            Console.Write("[" + this.Height + ", " + this.worker + "]");
            //Console.Write(string.Join("[,]", represent.ToArray()));
        }
    }
}