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
        static void Main(string[] args)
        {
            char[] starters = { '[', '{' };
            char[] enders = { ']', '}' };
            Stack<JToken> results = new Stack<JToken>();
            string line;
            string result = "";
            int balance = 0;
            int dquoteBalance = 0;
            int openArrBracket = 0;
            int closeArrBracket = 0;
            int openObjBracket = 0;
            int closeObjBracket = 0;

            while ((line = Console.ReadLine()) != null)
            {
                // using linq
                // set as different variable for each starters and enders
                // if the sum is 0, that means string or number

                // positive balance
                openArrBracket = line.Count(x => x == '[');
                openObjBracket = line.Count(x => x == '{');
                // negative balance
                closeArrBracket = line.Count(x => x == ']');
                closeObjBracket = line.Count(x => x == '}');
                var dquoteCount = line.Count(x => x == '"');

                result += line;

                balance = balance + openArrBracket + openObjBracket - closeArrBracket - closeObjBracket;
                dquoteBalance += dquoteCount;

                if (balance != 0 || dquoteBalance%2 != 0)
                {
                    continue;
                }

                if (balance == 0 || dquoteCount%2 == 0) // array, object, string, bool
                {
                    try
                    {
                        JToken o = JToken.Parse(result);
                        results.Push(o);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                result = "";
            }

            // print the stack from the top
            JsonEncode.PrintOut(results);

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