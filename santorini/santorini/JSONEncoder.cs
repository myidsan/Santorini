using System;
using System.Collections.Generic;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace santorini
{
    class JSONEncoder
    {
        JsonEncode printer = new JsonEncode();
        public JArray JSONParser()
        {
            Queue<JArray> test = new Queue<JArray>();
            JArray results = null;
            string line;
            string result = "";

            while ((line = Console.ReadLine()) != null)
            {
                result += line;
                try
                {
                    results = JArray.Parse(result);
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

        public class JsonEncode
        {
            public int index;
            public JArray value;

            public void PrintOut(Queue<JArray> results)
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