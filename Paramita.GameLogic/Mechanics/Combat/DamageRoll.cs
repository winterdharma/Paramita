using System;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;

namespace Paramita.GameLogic.Mechanics
{
    /*
        * This class conducts a DamageRoll after a successful AttackRoll hit
        * It currently is missing the case of a head hit, which uses defender's head protection only
        */
    class DamageRoll
    {
        Dice dice2d6;
        Actor attacker;
        Actor defender;
        int defenderProtection;
        Weapon attackWeapon;
        int attackRollResult;
        int attackScore;
        int defenseScore;

        public int Damage
        {
            get { return attackScore - defenseScore; }
        }
       



        public DamageRoll(AttackRoll attackRoll)
        {
            defender = attackRoll.Defender;
            attacker = attackRoll.Attacker;
            attackWeapon = attackRoll.AttackWeaon;
            attackRollResult = attackRoll.Result;

            dice2d6 = new Dice(2);

            ResolveDamageRoll();
        }




        private void ResolveDamageRoll()
        {
            defenderProtection = CalculateDefenderProtection();

            attackScore = attacker.Strength + attackWeapon.Damage + dice2d6.OpenEndedDiceRoll();
            defenseScore = defenderProtection + dice2d6.OpenEndedDiceRoll();
        }


        private int CalculateDefenderProtection()
        {
            int protection = defender.Protection;
            int shieldProtection = defender.ShieldProtection;

            if (CheckForShieldHit())
                protection += shieldProtection;

            if (CriticalHitCheck())
                protection /= 2;

            return protection;
        }

        
        private bool CheckForShieldHit()
        {
            if (attackRollResult <= defender.Parry)
            {
                Console.WriteLine("Shield hit.");
                return true;
            }
            return false;
        }

        
        private bool CriticalHitCheck()
        {
            int fatiguePenalty = defender.FatigueCriticalPenalty;
            int threshold = 3;
            int criticalCheckRoll = dice2d6.OpenEndedDiceRoll() - fatiguePenalty;
            if (criticalCheckRoll < threshold)
            {
                //GameScene.PostNewStatus("A critical hit was scored! (" + (criticalCheckRoll - fatiguePenalty)
                //    + "-" + fatiguePenalty + ")");
                return true;
            }
            return false;
        }
    }
}
