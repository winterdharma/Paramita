using System;

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
    *   There are two kinds of dice rolls: Open-ended and closed-ended.
    *   
    *   Open-ended dice rolls get bonus rolls whenever any roll is the max for that die size.
    *   Example: Roll 2 d6 dice. Both roll 6. An open-ended roll means 2 more bonus rolls are
    *            made and added to final total. Bonus rolls can yield more bonus rolls.
    *   
    *   Closed-ended dice rolls only roll once and return the result.
    */
    public class Dice
    {
        private Random random;
        private bool openEndedRoll;
        private Die dieSize;
        private int numberOfDice;
        private int rollModifier;
        private int diceLeftToRoll;




        public Dice(int numberOfDice, Die die = Die.d6, int modifier = 0)
        {
            dieSize = die;
            this.numberOfDice = numberOfDice;
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
            diceLeftToRoll = numberOfDice;
            int totalRolled = 0;
            int roll;

            while (diceLeftToRoll > 0)
            {
                roll = RollDie();
                if (openEndedRoll)
                    CheckForBonusRoll(roll);
                totalRolled += roll;
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
