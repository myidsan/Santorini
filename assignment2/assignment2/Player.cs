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

        private static void DeclareNumber(int num)
        {
            if (num < 1 || num > 10)
            {
                Console.WriteLine("error: 1 to 10 only. Try Again.");
                return;
            }
            Player_number = num;
            //Console.WriteLine("Player: Declared number as {0}!", num);
        }

        private static int GetNumber()
        {
            //Console.WriteLine("Player: Got number: {0}!", Player_number);
            Console.WriteLine(Player_number);
            return Player_number;
        }

        private static void SwitchNumber(bool choice)
        {
            Player_has_switched = choice;
            //Console.WriteLine("Player: Declared SwitchNumber {0}!", Player_has_switched);
            return;
        }
        private static bool HasPlayerSwitched()
        {
            //Console.WriteLine("Player: HasPlayerSwitched called");
            Console.WriteLine(Player_has_switched);
            return Player_has_switched;
        }

        public static void RunCommand(JObject json)
        {
            string operationName = json["operation-name"].ToString();

            // need to check number of inputs params? where?
            switch (operationName)
            {
                case "DeclareNumber":
                    int num = (int)json["operation-argument1"];
                    DeclareNumber(num);
                    break;

                case "GetNumber":
                    GetNumber();
                    break;

                case "SwitchNumber":
                    bool choice = (bool)json["operation-argument1"];
                    SwitchNumber(choice);
                    break;

                case "HasPlayerSwitched":
                    HasPlayerSwitched();
                    break;

                default:
                    break;
            }
            return;
        }
    }
}

