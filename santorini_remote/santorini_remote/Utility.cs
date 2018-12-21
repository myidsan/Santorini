using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini_remote
{
    public class Utility
    {
        public Utility()
        {
        }

        public static int ReadLookAhead()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = System.IO.Path.Combine(currentDirectory, "strategy.config");
            StreamReader file = File.OpenText(filePath);
            JsonTextReader reader = new JsonTextReader(file);
            JObject o2 = (JObject)JToken.ReadFrom(reader);
            int numPlays = Convert.ToInt32(o2["look-ahead"]);

            return numPlays;
        }

        /// <summary>
        /// Convert ArrayList to List.
        /// </summary>
        public static List<T> ToList<T>(ArrayList arrayList)
        {
            List<T> list = new List<T>(arrayList.Count);
            foreach (T instance in arrayList)
            {
                list.Add(instance);
            }
            return list;
        }

        ///// <summary>
        ///// Console output checker
        ///// Can be utilized in unit test to see the console output 
        ///// while redirecting the output to a string writer for comparison
        ///// and maintain actual output to the console
        ///// </summary>
        //public class ConsoleOutput : IDisposable
        //{
        //    private StringWriter stringWriter;
        //    private TextWriter originalOutput;

        //    public ConsoleOutput()
        //    {
        //        stringWriter = new StringWriter();
        //        originalOutput = Console.Out;
        //        Console.SetOut(stringWriter);
        //    }

        //    public string GetOuput()
        //    {
        //        return stringWriter.ToString();
        //    }

        //    public void Dispose()
        //    {
        //        Console.SetOut(originalOutput);
        //        stringWriter.Dispose();
        //    }
        //}
    }
}
