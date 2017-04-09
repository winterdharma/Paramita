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

        public AttackRoll(Combatant attacker, Weapon weapon, Combatant defender) : base(2)
        {
            _attacker = attacker;
            _attackWeapon = weapon;
            _defender = defender;

            _attackScore = OpenEndedRoll( 
                new List<int>() { attacker.AttackSkill, weapon.AttackModifier },
                new List<int>() { attacker.FatigueAttPenalty }
                );
            _defendScore = OpenEndedRoll(
                new List<int>() { defender.TotalDefense },
                new List<int>() { defender.FatigueDefPenalty, (defender.TimesAttacked - 1) * 2 }
                );

            _attackRollReport = CreateReport();
        }

        #region Properties
        public Combatant Defender
        {
            get { return _defender; }
        }
        public Combatant Attacker
        {
            get { return _attacker; }
        }
        public Weapon AttackWeapon
        {
            get { return _attackWeapon; }
        }
        public int Result
        {
            get { return _attackScore - _defendScore; }
        }
        public List<string> AttackRollReport { get { return _attackRollReport; } }
        public string AttackerReport
        {
            get { return _attackRollReport[0]; }
        }
        public string DefenderReport
        {
            get { return _attackRollReport[1]; }
        }
        #endregion

        #region Helper Methods
        private List<string> CreateReport()
        {
            var report = new List<string>();
            report.Add(_attacker + " rolled " + _attackScore + "(attSkill: " + _attacker.AttackSkill
                + ", fatigue: " + _attacker.FatigueAttPenalty + ")");
            report.Add(_defender + " rolled " + _defendScore + "(defSkill: " + _defender.TotalDefense
                + ", fatigue: " + _defender.FatigueDefPenalty + ", multAtt: " + ((_defender.TimesAttacked - 1) * 2) + ")");
            return report;
        }
        #endregion
    }
}
