using System;

namespace assignment2
{
    public class Player
    {
        int player_number;
        public int Player_number { get => player_number; set => player_number = value; }

        bool player_has_switched = false;
        public bool Player_has_switched { get => player_has_switched; set => player_has_switched = value; }

        public void DeclareNumber(int num)
        {
            // need to check number of inputs params? where?
            if (num < 1 || num > 10)
            {
                Console.WriteLine("error: 1 to 10 only");
                return;
            }
            Player_number = num;
            Console.WriteLine("Player: Declared number as {0}!", num);
        }

        public int GetNumber()
        {
            Console.WriteLine("Player: Got number: {0}!", Player_number);
            return Player_number;
        }

        public void SwitchNumber(bool choice)
        {
            // how to check param format
            Player_has_switched = true;
            Console.WriteLine("Player: Declared choose {0}!", choice);
            return;
        }
        public bool HasPlayerSwitched()
        {
            Console.WriteLine("Player: HasPlayerSwitched called");
            return Player_has_switched;
        }
    }
}

