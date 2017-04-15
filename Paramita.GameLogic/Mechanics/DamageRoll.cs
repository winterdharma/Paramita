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


        public DamageRoll(int attackResult, int weaponDmg, Combatant attacker, Combatant defender, 
            IRandom stubbedRandom = null) : base(2)
        {
            if (stubbedRandom != null)
                Random = stubbedRandom;

            ResolveDamageRoll(attackResult, attacker, weaponDmg, defender);
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
        private void ResolveDamageRoll(int attackResult, Combatant attacker, int weaponDmg, 
            Combatant defender)
        {
            int defProtection = GetEffectiveProtection(attackResult, attacker, defender);
            int strength = attacker.Strength;

            _attackScore = OpenEndedRoll(strength, weaponDmg);
            _defenseScore = OpenEndedRoll(defProtection);

            _damageRollReport.Add("Attacker damage score was " + _attackScore + " (Strength: " 
                + strength + ", Weapon damage: " + weaponDmg + ")");
            _damageRollReport.Add("Defender protection score was " + _defenseScore + " (Protection: " 
                + defProtection + ")");
        }


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
            int criticalCheckRoll = OpenEndedRoll(-defFatiguePenalty);

            if (criticalCheckRoll < threshold)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
