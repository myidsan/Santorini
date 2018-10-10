using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            JToken JSONcommand = null;
            Player newPlayer = new Player(5, false);

            // Dispatch
            while (true)
            {
                JSONcommand = JSONEncoder.JSONParser();
                if (JSONcommand == null) break;
                JObject command = JObject.Parse(JSONcommand.ToString());
                //var playerOperation = command["operation-name"];
                //Console.WriteLine(playerOperation); // DeclareNumber

                //List<dynamic> o = new List<dynamic>();


                //Console.WriteLine(command.Properties());
                //foreach (var item in command.Properties())
                //{
                //    //Console.WriteLine(item);
                //    if (item.Name == "operation-name") continue;
                //    o.Add(item.Value);
                //}
                //object[] arguments = o.ToArray();

                newPlayer.RunCommand(command);

                //Type playerType = Type.GetType("Player");
                //Console.WriteLine(playerType);
                //Console.WriteLine("1");
                //ConstructorInfo playerConstructor = playerType.GetConstructor(Type.EmptyTypes);
                //Console.WriteLine("2");
                //object playerCommandObject = playerConstructor.Invoke(new object[] { });
                //Console.WriteLine("3");

                //MethodInfo playerMethod = playerType.GetMethod(command["operation-name"].ToString());
                //Console.WriteLine(playerMethod);
                //object playerResult = playerMethod.Invoke(playerCommandObject, arguments);

            }

            return;
        }
    }
}
