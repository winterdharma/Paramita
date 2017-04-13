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
        private MoraleCheckType _checkType;
        #endregion


        public MoraleCheck(Combatant checker, MoraleCheckType type, Combatant opponent = null, 
            int modifier = 0, IRandom stubbedRandom = null) : base(2)
        {
            if (stubbedRandom != null)
                Random = stubbedRandom;

            _checkType = type;
            _isSuccessful = ResolveMoraleCheck(checker, opponent, modifier);
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

        private bool ResolveMoraleCheck(Combatant checker, Combatant opponent, int modifier)
        {
            int target = GetTargetValue(_checkType);
            int sizeDifference = GetSizeDifference(checker, opponent);
            int morale = checker.Morale;

            int checkerTotal = OpenEndedRoll(morale, sizeDifference);
            int checkAgainstTotal = OpenEndedRoll(target, modifier);

            _moraleCheckReport.Add("Checker total: " + checkerTotal
                + " (Morale: " + morale + ", Size difference: " + sizeDifference + ")");
            _moraleCheckReport.Add("Target total: " + checkAgainstTotal
                + " (Target: " + target + ", Modifier: " + modifier + ")");

            var result = GetResults(checkerTotal - checkAgainstTotal);
            _moraleCheckReport.Add(result.Item2);
            return result.Item1;
        }

        private int GetSizeDifference(Combatant checker, Combatant opponent)
        {
            return opponent != null ? checker.Size - opponent.Size : 0;
        }

        private Tuple<bool, string> GetResults(int difference)
        {
            if (difference > 0)
                return new Tuple<bool, string>(true, "Checker passed the morale check!");
            else
                return new Tuple<bool, string>(false, "Checker failed the morale check!");
        }
        #endregion
    }
}
