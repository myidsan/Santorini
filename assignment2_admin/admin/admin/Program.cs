using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace admin
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the commands from STDIN
            JSONEncoder JSONEcoder = new JSONEncoder();
            Admin newAdmin = new Admin();
            newAdmin.StartGame();

            return;
        }
    }
}
