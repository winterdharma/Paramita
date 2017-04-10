using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Mechanics
{
    public class DamageRoll : Dice
    {
        #region Fields
        private int _attackScore;
        private int _defenseScore;
        private List<string> _damageRollReport = new List<string>();
        #endregion


        public DamageRoll(int attackResult, int weaponDmg, Combatant attacker, Combatant defender) 
            : base(2)
        {
            int defProtection = GetEffectiveProtection(attackResult, attacker, defender);

            _attackScore = OpenEndedRoll(new List<int>() { attacker.Strength, weaponDmg });
            _defenseScore = OpenEndedRoll(new List<int>() { defProtection });
        }


        #region Properties
        public int Damage
        {
            get
            {
                int damage = _attackScore - _defenseScore;
                if (damage > 0)
                    return damage;
                else
                    return 0;
            }
        }

        public List<string> DamageRollReport { get { return _damageRollReport; } }
        #endregion


        #region Helper Methods
        private int GetEffectiveProtection(int attackResult, Combatant attacker, Combatant defender)
        {
            int protection = defender.Protection;

            if (ShieldWasHit(attackResult, defender.Parry))
            {
                _damageRollReport.Add(defender + "'s shield was hit.");
                protection += defender.ShieldProtection;
            }

            if (WasCriticalHit(defender.FatigueDefPenalty))
            {
                _damageRollReport.Add(attacker + " scored a critical hit!");
                protection /= 2;

            }

            return protection;
        }

        private bool ShieldWasHit(int attackResult, int defParry)
        {
            if (attackResult <= defParry)
            {
                return true;
            }
            return false;
        }

        private bool WasCriticalHit(int defFatiguePenalty)
        {
            int threshold = 3;
            int criticalCheckRoll = OpenEndedRoll(null, new List<int>() { defFatiguePenalty });

            if (criticalCheckRoll < threshold)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
