using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace assignment1
{
    class Program
    {
        static void Main()
        {
            string path = @"tempText.txt";
            StreamWriter tw = new StreamWriter(path);

            while (true)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    tw.Close();
                    break;
                }
                tw.WriteLine(input);
            }

            //// File
            StreamReader file = new StreamReader(@"./tempText.txt");
            char[] starters = { '[', '{' };
            char[] enders = { ']', '}' };
            Stack<JToken> results = new Stack<JToken>();
            string line;
            string result = "";
            int balance = 0;

            while ((line = file.ReadLine()) != null)
            {
                // using linq
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
                        JToken o = JToken.Parse(result);
                        results.Push(o);
                    }
                    catch (Exception)
                    {
                        // string
                        var dquoteCount = line.Count(x => x == '"');
                        if (dquoteCount == 2)
                        {
                            JToken o = JToken.Parse(line);
                            results.Push(o);
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
                                JToken o = JToken.Parse(line);
                                results.Push(o);
                            }
                            catch (Exception) // multiple-line string or empty string
                            {
                                continue;
                            }
                        }
                    }
                    result = "";
                }
            }
            file.Close();

            // print the stack from the top
            JsonEncode.PrintOut(results);

            File.Delete(@"tempText.txt");
            return;
        }
    }

    public class JsonEncode
    {
        public int index;
        public JToken value;

        static public void PrintOut(Stack<JToken> results)
        {
            while (results.Count != 0)
            {
                JsonEncode JsonObject = new JsonEncode
                {
                    index = results.Count,
                    value = results.Pop()
                };
                string JSONresult = JsonConvert.SerializeObject(JsonObject);
                Console.WriteLine(JSONresult);

            }
            return;
        }
    }
}