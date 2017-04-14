using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Mechanics
{
    public class AttackRoll : Dice
    {
        #region Fields
        Combatant _attacker;
        Combatant _defender;
        Weapon _attackWeapon;
        int _attackScore;
        int _defendScore;
        List<string> _attackRollReport = new List<string>();
        #endregion

        public AttackRoll(Combatant attacker, Weapon weapon, Combatant defender, 
            IRandom stubbedRandom = null) : base(2)
        {
            if (stubbedRandom != null)
                Random = stubbedRandom;

            _attacker = attacker;
            _attackWeapon = weapon;
            _defender = defender;

            _attackScore = OpenEndedRoll(attacker.AttackSkill, weapon.AttackModifier, 
                -attacker.FatigueAttPenalty);
            _defendScore = OpenEndedRoll(defender.TotalDefense, -defender.FatigueDefPenalty, 
                -defender.TimesAttackedPenalty);

            _attackRollReport = CreateReport();
        }

        #region Properties
        public int Result
        {
            get { return _attackScore - _defendScore; }
        }
        public List<string> AttackRollReport { get { return _attackRollReport; } }
        #endregion


        #region Helper Methods
        private List<string> CreateReport()
        {
            var report = new List<string>();
            report.Add(_attacker + "'s attack score was " + _attackScore + "(Attack skill: " + _attacker.AttackSkill
                + "Weapon modifier: " + _attackWeapon.AttackModifier + ", Fatigue penalty: " + _attacker.FatigueAttPenalty + ")");
            report.Add(_defender + "'s defense score was " + _defendScore + "(Total defense: " + _defender.TotalDefense
                + ", Fatigue penalty: " + _defender.FatigueDefPenalty + ", Times attacked penalty: " 
                + _defender.TimesAttackedPenalty + ")");
            return report;
        }
        #endregion
    }
}
