using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace santorini_remote
{
    public class RemotePlayer
    {
        Player localPlayer = null;

        public RemotePlayer()
        {
            localPlayer = new Player();
        }

        //["Register"]
        public string Register()
        {
            return localPlayer.PlayerName;
        }

        //["Place", Color, Initial - Board]
        // reply with a JSON list that contains two pairs of numbers, each between 0 and 4.
        public List<List<int>> Place(string playerColor, Board initialBoard)
        {
            return localPlayer.Place(initialBoard, playerColor);
        }

        //["Play", Board]
        // reply with a JSON value of the form [Worker,Directions]. 
        // The value should correspond to a play that the player wants to execute.
        public ArrayList Play(Board board)
        {
            return localPlayer.Play(board);
        }

        //["Game Over", Name]
        public string GameOver()
        {
            return "OK";
        }

        public string RunCommand(JArray action)
        {
            Console.WriteLine("in runcommand");
            string methodName = action[0].ToString();

            // need to parse the command into C# name style
            string command = ParseMethodName(methodName);
            Console.WriteLine("command: " + command);
            MethodInfo mi = this.GetType().GetMethod(command);
            ParameterInfo[] parameters = mi.GetParameters();

            object[] args = new object[parameters.Length];
            List<object> argsList = new List<object>() { };

            for (int i = 0; i < parameters.Length; i++)
            {
                if (action[i + 1].GetType() == typeof(JValue))
                {
                    args[i] = action[i + 1].ToString();
                }
                else if (action[i + 1].GetType() == typeof(JArray))
                {
                    JArray board = (JArray)action[i + 1];
                    args[i] = new Board(board);
                }
            }
            object result = mi.Invoke(this, args);
            return JSONEncoder.DumpJson(result);
        }

        public string ParseMethodName(string name)
        {
            string[] parsing = name.Split(' ');
            string command = "";
            foreach (var item in parsing)
            {
                command += item.Substring(0, 1).ToUpper() + item.Substring(1);
            }
            return command;
        }
    }
}
