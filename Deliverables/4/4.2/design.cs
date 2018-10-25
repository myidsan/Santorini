using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class that interacts with the game-engine in the player perspective
/// </summary>
public class Player
{
    // maintain both player's workers and oppostive player's workers
    // to use them to choose strategy
    public static List<String> playerWorkers;
    public static List<String> oppWorkers;

    /// <summary>
    /// The player has to be able to register with the game engine
    /// </summary>
    /// Contract: 1. The player should be given a color from
    ///              the game engine in the form of a String
    /// Dependency: 1. PlaceWorker() to place the workers initially
    /// Knowledge: N/A
    public void RegisterPlayer() { return; }

    // send to the game engine its next play and
    /// <summary>
    /// Declares the player's play which would be fed into the game engine
    /// It interacts with the strategy component to get the best next play
    /// </summary>
    /// Contract: 1. gets the JArray representation of 
    ///              current board object as input
    ///           2. returns a JArray formatted command
    /// Dependency: 1. Strategy GetBestMove(JArray jarrayBoard) to get the best move
    /// Knowledge: 1. Json Value of the Board_ (JArray format)
    public JArray DeclarePlay(JArray jarrayBoard) { return new JArray(); }
}

/// <summary>
/// Logic that tells the next best move for the player
/// </summary>
public class Strategy
{
    /// <summary>
    /// The only public method that the Player class can access
    /// Calls the helper methods (below) in order to get the best play to win
    /// </summary>
    /// Contract: 1. recevies a valid JAarray board
    ///           2. 
    /// Dependency: N/A - all within the Strategy class
    /// Knowledge: N/A
    /// <returns>
    /// The best play for the player
    /// </returns>
    public JArray GetBestMove(JArray jarrayBoard) { return new JArray(); }

    /// <summary>
    /// Checks if moving a player's worker in the given list to an adjacent cell 
    /// will make the player win in one move
    /// </summary>
    /// Contract: 1. input param workers == Player.playerWorkers
    /// Dependency: 1. Board.getHeight() to check for level 3 buildling
    ///             2. RuleCheker.IsValidMove() to check if the worker 
    ///                can climb to level 3 building
    /// Knowledge: N/A
    /// <returns>
    /// JArray of the play [action, []] that will make the player win in one turn
    ///  </returns>
    private JArray WinInOneTurn(List<String> workers) { return new JArray(); }

    /// <summary>
    /// Prevents the in one turn.
    /// </summary>
    /// Contract: 1. input param workers == Player.oppWorkers
    /// Dependency: 1. Board.getHeight(), 2. RuleChecker.IsValidBuild()
    /// Knowledge: N/A
    /// <returns>
    /// the best play for the player in JArray format
    /// </returns>
    /// 
    /// 1. Run the WinInOneTurn for the oppWorkers
    /// 2. If the WinInOneTurn returns a non-empty JArray, (meaning that opp can win)
    ///    parse the return JArray value to get the oppWorker and its directions
    /// 3. calculate the targetCellPos (List<int>) by oppWorkerPos - moveDirection
    /// 4. Call IsValidPlay(oppWorker, targetCellPos)
    ///     - if true, targetCellPos - workerPos and lookup the List<int> in 
    ///       Board.reverseDirection dictionary.
    ///       - if key exists, return JArray of [worker, [moveDir, buildDir]]
    ///     - if false, call DefaultMove()
    private JArray PreventLoseInOneTurn(List<String> oppWorkers) { return new JArray(); }

    /// <summary>
    /// Gets a default move for the player when all above methods fail
    /// </summary>
    /// Contract: 1. input param param name="workers"== Player.playerWorkers
    /// Dependency: 1. RuleChecker.IsValidMove() and 2. RuleChecker.IsValidBuild()
    /// Knowledge: N/A
    /// <returns>
    /// returns a random valid move for the player
    /// </returns>
    private JArray DefaultMove(List<String> workers) { return new JArray(); }

    /// <summary>
    /// Checks if the player can either move or build on the given List<int> of coordinate
    /// </summary>
    /// Contract: 1. worker is in the Board.PlayerPosition
    /// Dependency: 1. RuleChecker.IsValidMove() and 2. RuleChecker.IsValidBuild()
    /// Knowledge: Board.PlayerPosition
    /// <returns>
    /// <c>true</c>, if the worker can move or build on given coordinate, 
    /// <c>false</c> otherwise.
    /// </returns>
    private bool IsValidPlay(String worker, List<int> coord) { return true; }
}
