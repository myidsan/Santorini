using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
// additional packages
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace admin
{
    class JSONEncoder
    {
        public JToken JSONParser(Socket socket)
        {
            JToken results = null;
            string line;
            string result = "";
            //int balance = 0;

            NetworkStream networkStream = new NetworkStream(socket);
            StreamReader stream = new StreamReader(networkStream);


            Console.WriteLine("Reading data...");
            while ((line = stream.ReadToEnd()) != "")
            {
                Console.WriteLine("waiting for end");
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