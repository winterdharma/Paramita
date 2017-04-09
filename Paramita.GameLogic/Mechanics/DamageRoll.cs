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


        public DamageRoll(AttackRoll attackRoll) : base(2)
        {
            int defProtection = GetEffectiveProtection(attackRoll);

            _attackScore = OpenEndedRoll(new List<int>()
                { attackRoll.Attacker.Strength, attackRoll.AttackWeapon.Damage });
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
        private int GetEffectiveProtection(AttackRoll attackroll)
        {
            var defender = attackroll.Defender;
            var attacker = attackroll.Attacker;
            int protection = defender.Protection;

            if (ShieldWasHit(attackroll.Result, defender.Parry))
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
