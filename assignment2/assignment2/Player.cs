using System;
using Newtonsoft.Json.Linq;


namespace assignment2
{

    public class Player
    {
        static int player_number;
        public static int Player_number { get => player_number; set => player_number = value; }

        static bool player_has_switched = false;
        public static bool Player_has_switched { get => player_has_switched; set => player_has_switched = value; }

        public Player(int num, bool choice)
        {
            Player_number = num;
            Player_has_switched = choice;
        } // constructor for test case

        private int DeclareNumber()
        {
            //int num = Convert.ToInt32(Console.ReadLine()); 
            //if (num < 1 || num > 10)
            //{
            //    Console.WriteLine("error: 1 to 10 only. Try Again.");
            //    return -1;
            //} // commented out for testing purpose
            //Player_number = num;
            Console.WriteLine(Player_number);
            return Player_number;
        }

        private int GetNumber()
        {
            Console.WriteLine(Player_number);
            return Player_number;
        }

        private bool SwitchNumber()
        {
            //bool choice = Convert.ToBoolean(Console.ReadLine()); // commented out for testing purpose
            Console.WriteLine(Player_has_switched);
            return Player_has_switched;
        }

        private bool HasPlayerSwitched()
        {
            Console.WriteLine(Player_has_switched);
            return Player_has_switched;
        }

        public void RunCommand(JObject json)
        {
            string operationName = json["operation-name"].ToString();

            // need to check number of inputs params? where?
            switch (operationName)
            {
                case "DeclareNumber":
                    DeclareNumber();
                    break;

                case "GetNumber":
                    GetNumber();
                    break;

                case "SwitchNumber":
                    SwitchNumber();
                    break;

                case "HasPlayerSwitched":
                    HasPlayerSwitched();
                    break;

                default:
                    Console.WriteLine("operation command not recognized");
                    break;
            }
            return;
        }
    }
}

