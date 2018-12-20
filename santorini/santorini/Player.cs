using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Player
    {
        public string playerColor = "";
        string oppColor = "";
        string playerName = "";
        public string OppColor { get => oppColor; }
        public string PlayerName { get => playerName; }

        // for player-test-harness
        public Player(string name="World")
        {
            playerName = name;
        }

        public string GetOpponentColor()
        {
            foreach (var pColor in workerColors)
            {
                if (pColor != playerColor)
                {
                    return pColor;
                }
            }
            return "";
        }

        //public List<string> GenerateWorkerName(Player targetPlayer)
        //{
        //    string workerColor = targetPlayer.PlayerColor;
        //    List<string> workers = new List<string>();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        workers.Add(workerColor + i.ToString());
        //    }
        //    return workers;
        //}

        List<List<int>> InitPlacementCandidates = new List<List<int>>
        {
            new List<int> {0, 0},
            new List<int> {0, 4},
            new List<int> {4, 4},
            new List<int> {4, 0}
        };

        List<string> workerColors = new List<string>
        {
            "blue", "white"
        };

        //public string RegisterPlayer()
        //{
        //    Random rand = new Random();
        //    int i = rand.Next(0, 2);
        //    return workerColors[i];
        //}

        public List<List<int>> Place(string color, Board board)
        {
            return PlacePlayerWorkers(board, color);
        }

        public ArrayList Play(Board board)
        {
            return Strategy.PreventLoseInNTurn(board, playerColor, oppColor);
            //return GetNextBestPlay(board, this.PlayerColor, this.OppColor);
        }

        // For placing the works in corners
        public List<List<int>> PlacePlayerWorkersCorners(Board board, string color) 
        {
            int count = 1;
            List<List<int>> coordinates = new List<List<int>>();
            foreach (var candidate in InitPlacementCandidates)
            {
                if (count == 3) break;
                if (RuleChecker.IsValidInitPlacement(board, candidate))
                {
                    string worker = color + Convert.ToString(count);
                    board.PlaceWorker(worker, candidate);
                    count++;
                    coordinates.Add(candidate);
                }
            }
            return coordinates;
        }

        // For placing the works in general position
        // need modification - 11/16/18
        public List<List<int>> PlacePlayerWorkers(Board board, string color)
        {
            int count = 1;
            List<List<int>> coordinates = new List<List<int>>();
            foreach (var candidate in InitPlacementCandidates)
            {
                if (count == 3) break;
                if (RuleChecker.IsValidInitPlacement(board, candidate))
                {
                    string worker = color + Convert.ToString(count);
                    board.PlaceWorker(worker, candidate);
                    count++;
                    coordinates.Add(candidate);
                }
            }
            return coordinates;
        }

        public ArrayList GetNextBestPlay(Board board, string playerColor, string oppColor)
        {
            return Strategy.GetNextBestPlayStrategy(board, playerColor, oppColor);
        }

        public static List<string> GetPlayerWorkers(Board board, string playerColor)
        {
            List<string> targets = new List<string>();
            foreach (var workerName in board.PlayerPosition.Keys)
            {
                if (workerName.Contains(playerColor))
                {
                    targets.Add(workerName);
                }
            }
            return targets;
        }

        public void RunCommand(JArray action)
        {
            string methodName = action[0].ToString();

            // need to parse the command into C# name style
            string command = ParseMethodName(methodName);
            MethodInfo mi = this.GetType().GetMethod(command);
            ParameterInfo[] parameters = mi.GetParameters();

            object[] args = new object[parameters.Length];
            List<object> argsList = new List<object>() { };

            for (int i = 0; i < parameters.Length; i++)
            {
                if (action[i+1].GetType() == typeof(JValue))
                {
                    args[i] = action[i + 1].ToString();
                }
                else if (action[i+1].GetType() == typeof(JArray))
                {
                    JArray board = (JArray)action[i + 1];
                    args[i] = new Board(board);
                }
            }
            object result = mi.Invoke(this, args);
            JSONEncoder.PrintJson(result);
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
