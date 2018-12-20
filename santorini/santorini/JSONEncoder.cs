using System;
using System.Collections.Generic;
using System.IO;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    class JSONEncoder
    {
        JsonEncode printer = new JsonEncode();
        public JToken JSONParser()
        {
            Queue<JToken> test = new Queue<JToken>();
            JToken results = null;
            string line;
            string result = "";

            while ((line = Console.ReadLine()) != null)
            {
                result += line;
                try
                {
                    results = JToken.Parse(result);
                    return results;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return results;
        }

        public static void PrintJson(object target)
        {
            string JSONresult = JsonConvert.SerializeObject(target);
            Console.WriteLine(JSONresult);
        }

        public static string DumpJson(object target)
        {
            string JSONresult = JsonConvert.SerializeObject(target);
            return JSONresult;
        }

        //public static object ReadJson(string path)
        //{
        //    string text = System.IO.File.ReadAllText(path);
        //    object result = JsonConvert.DeserializeObject(text); // bug
        //    return result;
        //}

        public class JsonEncode
        {
            public int index;
            public JToken value;

            public void PrintOut(Queue<JToken> results)
            {
                while (results.Count != 0)
                {
                    JsonEncode JsonObject = new JsonEncode
                    {
                        index = results.Count,
                        value = results.Dequeue()
                    };
                    string JSONresult = JsonConvert.SerializeObject(JsonObject);
                    //object JSON = JsonConvert.DeserializeObject(JSONresult);
                    Console.WriteLine(JSONresult);
                }
                return;
            }
        }
    }
}