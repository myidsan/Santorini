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
            Admin newAdmin = new Admin();
            newAdmin.StartGame();

            return;
        }
    }
}
