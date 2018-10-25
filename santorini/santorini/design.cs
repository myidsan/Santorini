using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;


// <summary>
// This class implements the board of the game.
// It contains necessary methods to for workers to move 
// and build on the board.
// it contains information that the board should be aware of.
// </summary>
public class Board
{
    static Cell[,] board = new Cell[5, 5];
    static public Cell[,] Board_ { get => board; set => board = value; }

    /// <summary>
    /// A dictionary to hold the player position
    /// </summary>
    static Dictionary<string, List<int>> playerPosition;
    static public Dictionary<string, List<int>> PlayerPosition { get => playerPosition; set => playerPosition = value; }

    /// <summary>
    /// A dictionary to hold the direction in {int, int} format
    /// </summary>
    static public Dictionary<string, List<int>> directions = new Dictionary<string, List<int>>() { };

    /// <summary>
    /// Checks if the cell in the direction adjacent of the worker exists
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary
    /// </summary>
    /// <returns>
    /// A boolean whether a cell adjacent to the worker 
    /// exists in the given direction
    /// </returns>
    bool NeighboringCellExists(String worker, String direction) { return true; }

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
    bool Occupied(String worker, String direction) { return true; }

    /// <summary>
    /// Increase the height of the cell by one
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           NeighboringCellExists --> true
    /// </summary>
    /// <returns>
    /// An integer representing the height of the cell
    /// </returns>
    int GetHeight(String worker, String direction) { return 0; }

    /// <summary>
    /// Move the given worker in the given direction
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           NeighboringCellExists --> true,
    ///           Occupied --> false
    /// </summary>
    /// <returns>
    /// The Board_ field in the Board class
    /// </returns>
    Cell[,] Move(String worker, String direction) { return Board_; }

    /// <summary>
    /// Increment the height of the adjacent cell of the worker in the given direction
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           NeighboringCellExists --> true,
    ///           Occupied --> false
    /// </summary>
    /// <returns>
    /// The Board_ field in the Board class
    /// </returns>
    Cell[,] Build(String worker, String direction) { return Board_; }


    //// helpers
    /// <summary>
    /// Calculates the desired position of the worker in relation to the given direction
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    /// </summary>
    /// <returns>
    /// A list of integers in {row, col} format
    /// </returns>
    List<int> GetDesiredPosition(String worker, string direction) { return new List<int> {}; }
    /// <summary>
    /// places a worker in the given grid coordinate
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           Occupied --> false
    /// </summary>
    /// <returns>
    /// returns nothing. Prints out the Board_ to the console
    /// </returns>
    void PlaceWorker(String Worker, int row, int col) { return; }

    // Debugging methods
    void PrintPlayerPosition(Dictionary<string, List<int>> PlayerPositionDictionary) { return; }
    void PrintBoard() { return; }
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

    public dynamic[] PrintCell() { return new List<dynamic>().ToArray(); } // looks really sketchy
}

public class RuleChecker
{
    public JValue MoveChecker(string worker, string[] direction)
    {
       
        if (NeighboringCellExistsHelper(worker, direction[0]))
        {
            return (JValue)"yes";
        }
        return (JValue)"No";
    }

    public bool NeighboringCellExistsHelper(string worker, string direction) { return true; }

    public bool OccupiedHelper(string worker, string direction) { return true; }

    public List<int> GetDesiredPosition(string worker, string direction) { return new List<int>(); }
}