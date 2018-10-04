using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
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
            Stack<dynamic> results = new Stack<dynamic>();
            string line;
            string result = "";
            int balance = 0;

            while ((line = file.ReadLine()) != null)
            {
                foreach (var character in line)
                {
                    if (Array.Exists(starters, element => element == character))
                    {
                        balance++;
                    }
                    else if (Array.Exists(enders, element => element == character))
                    {
                        balance--;
                    }
                    else if (character == ' ')
                    {
                        continue;
                    }
                    result += character;
                }
                if (balance == 0)
                {
                    try // handles object
                    {
                        JObject o = JObject.Parse(result);
                        results.Push(o);
                    }
                    catch (Exception) // handles array
                    {
                        try
                        {
                            JArray o = JArray.Parse(result);
                            results.Push(o);
                        }
                        catch (Exception)
                        {
                            // string
                            if (line == "")
                            {
                                continue;
                            }
                            if (line[0] == '"' && line[line.Length - 1] == '"')
                            {
                                results.Push(line.Substring(1, line.Length - 2));
                            }
                            else if (line[0] == '"' && line[line.Length - 1] != '"')
                            {
                                continue;
                            }
                            else if (line[0] != '"' && line[line.Length - 1] == '"')
                            {
                                results.Push(result.Substring(1, result.Length - 2));
                            }
                            else if (line[0] != '"' && line[line.Length - 1] != '"')
                            {
                                try
                                {
                                    double fval;
                                    if (Int32.TryParse(line, out int ival))
                                        results.Push(ival);
                                    else
                                    {
                                        fval = Convert.ToDouble(line);
                                        results.Push(fval);
                                    }
                                }
                                catch (Exception)
                                {
                                    if (line == "true" || line == "false" || line == "null")
                                    {
                                        JToken o = JToken.Parse(line);
                                        results.Push(o);
                                    }
                                    continue;
                                }
                            }
                        }

                    }
                    result = "";
                }
            }
            file.Close();

            // print the stack from the top
            while (results.Count != 0)
            {
                var jsonObject = new JObject
                    {
                        { "index", results.Count },
                        { "value", results.Pop() }
                    };
                Console.WriteLine(jsonObject);
            }
            File.Delete(@"tempText.txt");
            return;
        }
    }
}