using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace assignment2
{
    class JSONEncoder
    {
        public JToken JSONParser()
        {
            char[] starters = { '[', '{' };
            char[] enders = { ']', '}' };
            JToken results = null;
            string line;
            string result = "";
            int balance = 0;

            while ((line = Console.ReadLine()) != null)
            {
                //line = file.ReadLine();
                // using linq
                // set as different variable for each starters and enders
                // if the sum is 0, that means string or number

                // positive balance
                balance += line.Count(x => x == '[');
                balance += line.Count(x => x == '{');
                // negative balance
                balance -= line.Count(x => x == ']');
                balance -= line.Count(x => x == '}');
                result += line;

                if (balance == 0)
                {
                    try // array and object
                    {
                        results = JToken.Parse(result);
                        break;
                    }
                    catch (Exception)
                    {
                        // string
                        var dquoteCount = line.Count(x => x == '"');
                        if (dquoteCount == 2)
                        {
                            results = JToken.Parse(line);
                            break;
                        }
                        else if (dquoteCount == 1)
                        {
                            continue;
                        }
                        else
                        {
                            // parse non-string values: int, double, boolean, null
                            try
                            {
                                results = JToken.Parse(line);
                                break;
                            }
                            catch (Exception) // multiple-line string or empty string
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            return results;
        }
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