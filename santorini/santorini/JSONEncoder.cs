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