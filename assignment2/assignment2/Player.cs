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

        void DeclareNumber()
        {
            int num = 5; // hard-coded for test purpose
            //int num = Convert.ToInt32(Console.ReadLine()); 
            Player_number = num;
            if (num < 1 || num > 10)
            {
                Console.WriteLine("error: 1 to 10 only. Try Again.");
                return;
            }
            Console.WriteLine(Player_number);
            return;
        }

        int GetNumber()
        {
            Console.WriteLine(Player_number);
            return Player_number;
        }

        void SwitchNumber()
        {
            bool choice = true; // hard-coded for test purose
            //bool choice = Convert.ToBoolean(Console.ReadLine());
            Player_has_switched = choice;
            Console.WriteLine(Player_has_switched);
            return;
        }

        bool HasPlayerSwitched()
        {
            Console.WriteLine(Player_has_switched);
            return Player_has_switched;
        }

        public void RunCommand(JObject json)
        {
            string operationName = json["operation-name"].ToString();

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

