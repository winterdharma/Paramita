using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Mechanics
{
    public enum Die
    {
        d4 = 4,
        d6 = 6,
        d10 = 10,
        d12 = 12,
        d20 = 20,
        d100 = 100
    }


    /*
    *   Dice
    *   
    *   Currently, a dice roll must correspond to a code found in the ParseDiceCode() function!
    *
    *   @diceCode should use D&D conventions:
    *     * number of dice + "d" + max roll of die + any after-roll modifier (+/- integer)
    *   
    *   There are two kinds of dice rolls: Open-ended and closed-ended.
    *   
    *   Open-ended dice rolls get bonus rolls whenever any roll is the max for that die size.
    *   Example: Roll 2 d6 dice. Both roll 6. An open-ended roll means 2 more bonus rolls are
    *            made and added to final total. Bonus rolls can yield more bonus rolls.
    *   
    *   Closed-ended dice rolls only roll the initial dice indicated by the dice code.
    */
    public class Dice
    {
        private Random random;
        private bool openEndedRoll;
        private Die dieSize;
        private int diceLeftToRoll;
        private int rollModifier;




        public Dice(int dieRolls, Die die = Die.d6, int modifier = 0)
        {
            dieSize = die;
            diceLeftToRoll = dieRolls;
            rollModifier = modifier;
            random = new Random();
        }




        public int OpenEndedDiceRoll()
        {
            openEndedRoll = true;
            return RollDice();
        }



        public int ClosedEndedDiceRoll()
        {
            openEndedRoll = false;
            return RollDice();
        }



        private int RollDice()
        {
            int totalRolled = 0;
            int roll;
            while (diceLeftToRoll > 0)
            {
                roll = RollDie();
                if (openEndedRoll == true)
                    CheckForBonusRoll(roll);
            }
            return totalRolled;
        }



        private int RollDie()
        {
            diceLeftToRoll--;
            int roll = random.Next(1, (int)dieSize + 1) + rollModifier;
            Console.WriteLine("Rolled " + roll);
            return roll;
        }



        private void CheckForBonusRoll(int roll)
        {
            if (roll == (int)dieSize)
            {
                Console.WriteLine("Bonus roll!");
                diceLeftToRoll++;
            }
        }
    }
}
