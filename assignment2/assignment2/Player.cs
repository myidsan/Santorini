using System;

namespace assignment2
{
    public class Player
    {
        int player_number;
        //bool player_has_switched;

        public void DeclareNumber(int num)
        {
            do
            {
                try
                {
                    Console.WriteLine("in try");
                    num = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("FormatException. Enter an integer 1 through 10");
                }
            } while (num < 1 || num > 10);
            player_number = num;
        }

        public int GetNumber()
        {
            return player_number;
        }

        public void SwitchNumber(bool choice)
        {
            return;
        }
        public bool HasPlayerSwitched()
        {
            return false;
        }
    }
}
