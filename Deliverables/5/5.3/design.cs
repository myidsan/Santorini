using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class that runs the game and makes sure players are executing valid moves
/// </summary>
public class Referee
{

    // maintain both players' workers, current gameBoard, the moveOrder
    public static Player playerOne;
    public static Player playerTwo;
    public static Board gameBoard;
    private static Array moveOrder;
    private static bool isGameOver = false;
    private static string winner;


    /// <summary>
    /// Creates an empty, valid board and assigns gameBoard
    /// </summary>
    /// Contract: Returns an empty, valid board
    /// Dependency: 1. Board constructor
    ///             2. RuleChecker.isValidBoard()
    /// Knowledge: N/A
    public void initBoard(){ return gameBoard}


    /// <summary>   
    /// Receives two instances of the Player class and stores each of them in playerOne and playerTwo respectively
    /// Assigns playerOne and playerTwo different colors and assigns a random move order
    /// </summary>
    /// Contract: 
    /// Dependency: Player.PlacePlayerWorker()
    /// Knowledge: 
    public void registerPlayers(Player p1, Player p2){ return !isGameOver; }


    /// <summary>
    /// Calls Player.PlacePlayerWorkers on the gameBoard
    /// If Player tries to place a work illegally, set isGameOver to true
    /// </summary>
    /// Contract: 1. Valid player1 and Player 2 
    /// Dependency: 1. RuleChecker.isValidBoard() 
    ///             2. RuleChecker.IsValidInitPlacement()
    public void placeWorkers(Player p1, Player p2){ return !isGameOver;}


    /// <summary>
    /// Picks a random move from a player's list of best moves and manipulates the game board accordingly
    /// If play is illegal, set gameOver to true
    /// If playerBestMoves is illegal, return current isGameOver
    /// </summary>
    /// Contract: ArrayList of playerBestMoves contains valid moves
    /// Dependency: 1. RuleChecker.isValidBoard()
    ///             2. RuleChecker.isValidMove()
    ///             3. RuleChecker.isValidBUild()
    /// Knowledge: Game Board
    public void runPlay(ArrayList playerBestMoves){ return !isGameOver; }


    /// <summary>
    /// Wrapper for game play
    /// Calls functions in this order:
    ///         1. initBoard
    ///         2. registerPlayers
    ///         3. placeWorkers
    ///         4. While game not over, for each moveOrder, runPlay()
    /// </summary>
    /// Contract: 1.Board and Players have been correctly initialized
    ///           2. Move order not manipulated every iteration  
    /// Dependency: N/A
    /// Knowledge: isGameOver
    public void playGame(){ return; }


    /// <summary>
    /// Handles when game is over and notifies players on who won/lost
    /// </summary>
    /// Contract: isGameOver is set to true
    /// Dependency: N/A
    /// Knowledge: isGameOver
    public void handleGameOver(){ return;}
   
}