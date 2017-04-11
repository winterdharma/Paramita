using Paramita.GameLogic.Actors;
using System;
using System.Collections.Generic;

namespace Paramita.GameLogic.Mechanics
{
    public enum MoraleCheckType
    {
        RepelAttack,
        Test
    }

    public class MoraleCheck : Dice
    {
        #region Fields
        private bool _isSuccessful;
        private List<string> _moraleCheckReport = new List<string>();
        #endregion


        public MoraleCheck(Combatant checker, MoraleCheckType type, Combatant opponent = null, 
            int modifier = 0, IRandom stubbedRandom = null) : base(2)
        {
            if (stubbedRandom != null)
                Random = stubbedRandom;

            _isSuccessful = ResolveMoraleCheck(checker, opponent, GetTargetValue(type), modifier);
        }


        #region Properties
        public bool IsSuccessful
        {
            get { return _isSuccessful; }
        }

        public List<string> MoraleCheckReport
        {
            get { return _moraleCheckReport; }
        }
        #endregion


        #region Helper Methods
        private int GetTargetValue(MoraleCheckType type)
        {
            switch (type)
            {
                case MoraleCheckType.RepelAttack:
                    return 10;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool ResolveMoraleCheck(Combatant checker, Combatant opponent, int target, int modifier)
        {
            int sizeDifference = GetSizeDifference(checker, opponent);
            int morale = checker.Morale;

            int checkerTotal = OpenEndedRoll(new List<int>() { morale, sizeDifference });
            int checkAgainstTotal = OpenEndedRoll(new List<int>() { target, modifier });

            _moraleCheckReport.Add("Checker's total was " + checkerTotal
                + " (Morale: " + morale + ", SizeDiff: " + sizeDifference + ")");
            _moraleCheckReport.Add("Target total was " + checkAgainstTotal
                + " (Target: " + target + ", Modifier: " + modifier + ")");

            if (checkerTotal > checkAgainstTotal)
            {
                _moraleCheckReport.Add("Checker passed the morale check!");
                return true;
            }
            else
            {
                _moraleCheckReport.Add("Checker failed the morale check!");
                return false;
            }
        }

        private int GetSizeDifference(Combatant checker, Combatant opponent)
        {
            return opponent != null ? checker.Size - opponent.Size : 0;
        }
        #endregion
    }
}
