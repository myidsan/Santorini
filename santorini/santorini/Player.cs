﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Player
    {
        //public static List<String> playerWorkers;
        //public static List<String> OppWorkers;
        string playerColor = "";
        string oppColor = "";
        public string PlayerColor { get => playerColor; }
        public string OppColor { get => oppColor; }

        public Player()
        {
            playerColor = RegisterPlayer();
            oppColor = GetOpponentColor();
        }

        // for player-test-harness
        public Player(string color)
        {
            playerColor = color;
            oppColor = GetOpponentColor();
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

        List<List<int>> InitPlacementCandidates = new List<List<int>>
        {
            new List<int> {0, 0},
            new List<int> {0, 4},
            new List<int> {4, 4},
            new List<int> {4, 0}
        };

        List<string> workerColors = new List<string>
        {
            "Blue", "White"
        };

        public string RegisterPlayer()
        {
            Random rand = new Random();
            int i = rand.Next(0, 2);
            return workerColors[i];
        }

        public List<List<int>> Place(string color, Board board)
        {
            return PlacePlayerWorkers(board, color);
        }

        public ArrayList Play(Board board)
        {
            return GetNextBestPlay(board, this.PlayerColor, this.OppColor);
        }

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
            // player can win in one move
            if (Strategy.WinInOneTurn(board, playerColor).Count != 0)
                return Strategy.WinInOneTurn(board, playerColor);

            // player can prevent the opp from winning in one move
            if (Strategy.PreventLoseInOneTurn(board, playerColor, oppColor).Count != 0)
                return Strategy.PreventLoseInOneTurn(board, playerColor, oppColor);

            // player can't win and opp can't win in one move
            if (Strategy.WinInOneTurn(board, playerColor).Count == 0 && Strategy.WinInOneTurn(board, oppColor).Count == 0)
                return Strategy.DefaultPlay(board, playerColor);

            // player cannot prevent the opp from winning in one move
            return new ArrayList() { };
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