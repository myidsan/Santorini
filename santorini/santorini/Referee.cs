using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Referee
    {

        //Design a Santorini referee component.
        // 1. The referee has to be able to receive two instances of your player component that 
        // implements the interface for players you have designed and 
        // 2, manage a game of Santorini between these two players.
        // 3. If the referee detects that a player is attempting to violate the rules of the game it should abort the game 
        // and notify the cheating player that they lost and the other player that the won.

        Board internalBoard; // private as invariant
        int turn = 0; // 0 for blue, 1 for white
        bool firstPlayer = false;
        bool secondPlayer = false;
        List<Player> order = new List<Player>() { };
        enum workerColors { blue, white };

        public Board InternalBoard { get => internalBoard; }
        public List<Player> Order { get => order; set => order = value; }

        public Referee() {}

        public Referee(Player one, Player two)
        {
            one.playerColor = "blue";
            firstPlayer = true;
            two.playerColor = "white";
            secondPlayer = true;
            internalBoard = Board.InitBoard();
            order.Add(one);
            order.Add(two);
        }

        public string Name(string name)
        {
            if (turn == 0)
            {
                turn++;
                //firstPlayer = true;
                // set name of the player here? -- this sounds like a proxy player
                Console.WriteLine(JSONEncoder.DumpJson("blue"));
                return "blue";
            }
            else
            {
                turn--;
                //secondPlayer = true;
                // set name of the player here? -- this sounds like a proxy player
                // the referee doesn't need to have a reference copy of the player anyway
            }
            Console.WriteLine(JSONEncoder.DumpJson("white"));
            return "white";
        }

        public Cell[,] PlaceWorkers(JArray coords)
        { 
            // either player has not been initiazlied
            if (!(firstPlayer && secondPlayer))
            {
                throw new MissingFieldException("two players are need to play a game");
            }

            string playerColor = order[turn].playerColor;


            Board nextBoard = new Board(internalBoard.DumpBoard());
            List<string> workers = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                //List<int> targetCoord = Utility.ToList<int>((ArrayList)coords[i]);
                //List<int> targetCoord = coords.ToObject<List<int>>();

                List<List<int>> targetCoord = new List<List<int>>() { };
                targetCoord = coords.ToObject<List<List<int>>>();

                workers.Add(playerColor + (i+1).ToString());
                 //check for invalid placement
                if (!RuleChecker.IsValidPlacement(InternalBoard, workers[i], targetCoord[i]))
                {
                    if (turn == 0)
                    {
                        Console.WriteLine(JSONEncoder.DumpJson(order[turn + 1].PlayerName));
                    }
                    else 
                    {
                        Console.WriteLine(JSONEncoder.DumpJson(order[turn - 1].PlayerName));
                    }
                    // return the unchanged board for returning purpose
                    return null;
                }
                // place the worker in the board
                nextBoard.PlaceWorker(workers[i], targetCoord[i]);
            }
            if (turn == 1)
            {
                turn--;
            } else {
                turn++;
            }

            // update the internalBoard to the worker placed board
            internalBoard = nextBoard;

            Console.WriteLine(JSONEncoder.DumpJson(nextBoard.DumpBoard()));
            return nextBoard.Board_;
        }

        public Cell[,] ExecutePlay(string worker, string[] directions)
        {
            // play is legal and not a winning or losing
            // win is getting on top of level 3
            // invalid is either moving on occupied or moving up to 4 or building on occupied 
            // --> invalidPlay method make

            // illegal move - trying to move other player's worker
            if (!worker.Contains(order[turn].playerColor))
            {
                if (turn == 0)
                {
                    Console.WriteLine(JSONEncoder.DumpJson(order[turn + 1].PlayerName));
                }
                else
                {
                    Console.WriteLine(JSONEncoder.DumpJson(order[turn - 1].PlayerName));
                }
                return null;
            }

            // 1. winning move
            if (directions.Length == 1)
            {
                if (RuleChecker.IsPlayerWinnerAfterMove(this.internalBoard, worker, directions[0]))
                {
                    Console.WriteLine(JSONEncoder.DumpJson(order[turn].PlayerName));
                    return null;
                }
                else // player is making an illegal move
                {
                    if (turn == 0)
                    {
                        Console.WriteLine(JSONEncoder.DumpJson(order[turn + 1].PlayerName));
                    }
                    else
                    {
                        Console.WriteLine(JSONEncoder.DumpJson(order[turn - 1].PlayerName));
                    }
                    return null;
                }
            }

            // 2. losing move won't happen due to strategy implementation
            // 3. illegal play
            if (!RuleChecker.IsValidPlay(this.internalBoard, worker, directions))
            {
                if (turn == 0)
                {
                    Console.WriteLine(JSONEncoder.DumpJson(order[turn + 1].PlayerName));
                }
                else
                {
                    Console.WriteLine(JSONEncoder.DumpJson(order[turn - 1].PlayerName));
                }
                return null;

            }

            if (turn == 1)
            {
                turn--;
            }
            else
            {
                turn++;
            }

            // legal and valid plays
            this.internalBoard.Move(worker, directions[0]);
            this.internalBoard.Build(worker, directions[1]);
            Console.WriteLine(JSONEncoder.DumpJson(InternalBoard.DumpBoard()));
            return this.internalBoard.Board_;
        }
    }
}
