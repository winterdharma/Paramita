using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;

namespace Paramita.GameLogic.Actions
{
    /*
     * A morale check is a way to check if a being will decide not to proceed with
     * an action because his ability to overcome dread or fear fails.
     * 
     * A morale check proceeds like an AttackRoll except that the checker needs
     * to score better than a certain threshold that expresses the level of danger
     * he faces.
     */

    public enum MoraleCheckType
    {
        RepelAttack,
        Test
    }
    
    public class CheckMorale
    {
        #region Fields
        private Dice _dice2d6;
        private int _morale;
        private int _modifier;
        private int _target;
        private int _sizeDifference;
        private bool _isSuccessful;
        private List<string> _report = new List<string>();
        #endregion

        
        public CheckMorale(Actor checker, MoraleCheckType type, Actor opponent = null, int modifier = 0, Dice customDice = null)
        {
            // enables stubbing the RNG with contrived data for unit testing
            _dice2d6 = customDice ?? new Dice(2);

            _morale = checker.Morale;
            _modifier = modifier;
            _target = GetTargetValue(type);
            _sizeDifference = GetSizeDifference(checker, opponent);
            _isSuccessful = ResolveMoraleCheck();
        }


        #region Properties
        public bool IsSuccessful
        {
            get { return _isSuccessful; }
        }

        public List<string> Report
        {
            get { return _report; }
        }
        #endregion


        #region Helper Methods
        private int GetTargetValue(MoraleCheckType type)
        {
            switch(type)
            {
                case MoraleCheckType.RepelAttack:
                    return 10;
                default:
                    throw new NotImplementedException();
            }
        }

        private int GetSizeDifference(Actor checker, Actor opponent)
        {
            return opponent != null ? checker.Size - opponent.Size : 0;
        }

        private bool ResolveMoraleCheck()
        {
            int checkerTotal = _dice2d6.OpenEndedRoll() + _morale + _sizeDifference;

            _report.AddRange(_dice2d6.Report);
            _report.Add("Checker's morale was " + _morale + 
                " and size difference was " + _sizeDifference);
            _report.Add("Checker's total was " + checkerTotal);

            int checkAgainstTotal = _dice2d6.OpenEndedRoll() + _target + _modifier;

            _report.AddRange(_dice2d6.Report);
            _report.Add("Target value was " + _target +
                " and modifier was " + _modifier);
            _report.Add("Target total was " + checkAgainstTotal);

            if (checkerTotal > checkAgainstTotal)
            {
                _report.Add("Checker passed the morale check!");
                return true;

            }
            else
            {
                _report.Add("Checker failed the morale check!");
                return false;
            }
        }
        #endregion
    }
}
