using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.GameLogic.Mechanics
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
        #region Fields
        private IRandom _random = Dungeon._random;
        private Die _dieSize;
        private int _numberOfDice;
        private int _rollModifier;
        protected List<string> _report = new List<string>();
        #endregion

        public Dice(int dice, Die die = Die.d6, int modifier = 0)
        {
            _dieSize = die;
            _numberOfDice = dice;
            _rollModifier = modifier;
        }

        #region Property
        // Added to enable stubbing of RandomNum in unit tests
        public IRandom Random
        {
            set { _random = value; }
        }

        public List<string> Report
        {
            get { return _report; }
            private set { _report = value; }
        }
        #endregion

        
        public int OpenEndedRoll(List<int> additions = null, List<int> subtractions = null)
        {
            int modifiers = additions != null ? additions.Sum() : 0;
            modifiers -= subtractions != null ? subtractions.Sum() : 0;
            return RollDice(true) + modifiers;
        }

        public int ClosedEndedRoll()
        {
            return RollDice(false);
        }


        #region Helper Methods
        private int RollDice(bool isOpenEnded)
        {
            _report.Clear();
            int diceLeftToRoll = _numberOfDice;
            int totalRolled = 0;
            int roll;

            while (diceLeftToRoll > 0)
            {
                diceLeftToRoll--;
                roll = RollDie();
                if (isOpenEnded && CheckForBonusRoll(roll))
                {
                    diceLeftToRoll++;
                }
                totalRolled += roll;
            }

            Report.Add("Total: " + totalRolled);
            return totalRolled;
        }

        private int RollDie()
        {
            int roll = _random.Next(1, (int)_dieSize + 1) + _rollModifier;
            Report.Add("Roll " + _dieSize + ": " + roll);
            return roll;
        }

        private bool CheckForBonusRoll(int roll)
        {
            if (roll == (int)_dieSize)
            {
                Report.Add("Bonus Roll!");
                return true;
            }
            return false;
        }
        #endregion
    }
}
