using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

/// <summary>
/// This class implements the board of the game.
/// It contains necessary methods to for workers to move 
/// and build on the board.
/// it contains information that the board should be aware of.
/// </summary>
class Board
{
    IList<Row> board = new List<Row>() {
                new Row(),
                new Row(),
                new Row(),
                new Row(),
                new Row()
            };
    // dictionary_ key: String worker, value: List<int> {row, col}
    Dictionary<string, List<int>> playerPosition;

    // dictionary_  key: String worker, value: List<int> ex N: {-1, 0}
    Dictionary<string, List<int>> directions;
    
    /// <summary>
    /// Checks if the cell in the direction adjacent of the worker exists
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary
    /// </summary>
    /// <returns>
    /// A boolean whether a cell adjacent to the worker 
    /// exists in the given direction
    /// </returns>
    public bool NeighboringCellExists(String worker, String direction) { return true; }

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
    public bool Occupied(String worker, String direction) { return true; }

    /// <summary>
    /// Increase the height of the cell by one
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           NeighboringCellExists --> true
    /// </summary>
    /// <returns>
    /// An integer representing the height of the cell
    /// </returns>
    public int GetHeight(String worker, String direction) { return 1; }

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
    public Board Move(String worker, String direction) { return this; }

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
    public Board Build(String worker, String direction) { return this; }


    //// helpers

    /// <summary>
    /// Calculates the desired position of the worker in relation to the given direction
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    /// </summary>
    /// <returns>
    /// A list of integers in {row, col} format
    /// </returns>
    List<int> GetDesiredPosition(String worker, string direction) { return new List<int>(); }

    /// <summary>
    /// places a worker in the given grid coordinate
    /// contract: worker should be in the playerPosition dictionary, 
    ///           direction should be in the directions dictionary,
    ///           Occupied --> false
    /// </summary>
    /// <returns>
    /// The state of the board
    /// </returns>
    public void PlaceWorker(String Worker, int row, int col) { return; }

    // Debugging methods
    public void PrintPlayerPosition(Dictionary<string, List<int>> playerPosition) { return; }
    public void PrintBoard() { return; }
}

/// <summary>
/// Rows consists a board.
/// </summary>
class Row
{
    public IList<Cell> row = new List<Cell>() {
                new Cell(),
                new Cell(),
                new Cell(),
                new Cell(),
                new Cell()
            };
}

/// <summary>
/// Cells consists a row.
/// </summary>
class Cell
{
    private int height = 0;
    private string worker = null;
}