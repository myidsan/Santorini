using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace admin
{
    class JSONEncoder
    {
        public static JToken JSONParser()
        {
            JToken results = null;
            string line;
            string result = "";
            //int balance = 0;

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

            static public void PrintOut(Queue<JToken> results)
            {
                while (results.Count != 0)
                {
                    JsonEncode JsonObject = new JsonEncode
                    {
                        index = results.Count,
                        value = results.Dequeue()
                    };
                    string JSONresult = JsonConvert.SerializeObject(JsonObject);
                    Console.WriteLine(JSONresult);

                }
                return;
            }
        }
    }
}