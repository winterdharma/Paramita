using System.Collections.Generic;
using Paramita.GameLogic.Mechanics;

namespace Paramita.GameLogic.Actors.Combat
{
    /*
    *  Repel Meelee Attack
    *  
    *  When a defender is attacked, he can attempt to repel the attack if it has a weapon
    *  with greater length than the attack weapon.
    *  
    *  If so, the defender attempts to interrupt the original attack with a repel attack.
    *  
    *  If the repel attack is successful, the attacker rolls a morale check to see if
    *  it gives up on the original attack.
    *  
    *  If the attacker passes the morale check, it continues to attack, but the defender
    *  did hit it with the repel attack and a damage roll is made. If the damage is greater
    *  than zero, the attacker takes damage equal to @REPEL_ATTACK_DAMAGE.
    *  
    *  If the attacker happened to be killed by the repel attack damage, his attack
    *  is aborted.
    */
    internal class RepelMeleeAttack
    {
        #region Fields
        private const int REPEL_ATTACK_DAMAGE = 1;
        private readonly string REPEL_DAMAGE_REPORT = " takes " + REPEL_ATTACK_DAMAGE + " pt of damage!";
        private const string ATTACKER_KILLED = " is killed!";
        #endregion

        #region Properties
        public int Damage { get; set; }

        public List<string> Report { get; set; }
        #endregion

        public RepelMeleeAttack()
        {
            Report = new List<string>();
        }


        public bool Possible(Attack attack, Combatant defender)
        {
            Report.Clear();
            var repelWeapon = defender.GetLongestWeapon();
            var attackWeapon = attack.Weapon;

            int length = repelWeapon != null ? repelWeapon.Length : 0;

            if (length > attackWeapon.Length)
            {
                Report.Add(defender + " attempts to repel with " + repelWeapon.DisplayText());
                return true;
            }
            else
            {
                Report.Add(defender + " isn't able to repel attack.");
                return false;
            }
        }


        public bool Execute(Combatant defender, Attack attack, Combatant attacker)
        {
            Report.Clear();

            defender.AddAttackEncumbranceToFatigue();
            attacker.AddDefenseEncumbranceToFatigue();

            var repelWeapon = defender.GetLongestWeapon();

            var attackRoll = new AttackRoll(defender, repelWeapon, attacker);
            Report.AddRange(attackRoll.AttackRollReport);

            return ResolveRepelAttackResult(attackRoll.Result, attack.Weapon.Damage, attacker, 
                defender);
        }

        private bool ResolveRepelAttackResult(int attackResult, int weaponDmg, Combatant attacker, 
            Combatant defender)
        {
            bool attackRepelled = false;
            if (attackResult > 0)
            {
                attackRepelled = RepelAttackMoraleCheck(attackResult, weaponDmg, attacker, defender);
            }
                
            else
                Report.Add(defender + "'s repel attack missed!");

            return attackRepelled;
        }

        // If the attacker passes his morale check, he continues his attack
        // and is hit by the repel weapon used to try to stop him.
        // If attacker fails the morale check, he was stymied and aborts.
        private bool RepelAttackMoraleCheck(int attackResult, int weaponDmg, Combatant attacker,
            Combatant defender)
        {
            int checkModifier = attackResult / 2;
            var moraleCheck = new MoraleCheck(attacker, MoraleCheckType.RepelAttack, defender, 
                checkModifier);

            if (moraleCheck.IsSuccessful)
            {
                RepelAttackDamageRoll(attackResult, weaponDmg, attacker, defender);
                Report.Add(defender + " didn't stop the attack, but hit " + attacker + " for " 
                    + Damage + " dmg.");
                return false;
            }
            else
            {
                Report.Add(defender + " succeeded in repelling the attack.");
                return true;
            }
        }

        private void RepelAttackDamageRoll(int attackResult, int weaponDmg, Combatant attacker,
            Combatant defender)
        {
            var damageRoll = new DamageRoll(attackResult, weaponDmg, attacker, defender);
            Report.AddRange(damageRoll.DamageRollReport);

            if (damageRoll.Damage > 0)
            {
                Damage = REPEL_ATTACK_DAMAGE;
                Report.Add(defender + REPEL_DAMAGE_REPORT);
            }
            else
            {
                Damage = 0;
            }
        }
    }
}
