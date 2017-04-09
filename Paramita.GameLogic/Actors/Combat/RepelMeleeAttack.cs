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
            var repelWeapon = defender.GetLongestWeapon();

            var attackRoll = new AttackRoll(defender, repelWeapon, attacker);
            Report.AddRange(
                new string[] { attackRoll.AttackerReport, attackRoll.DefenderReport });

            return ResolveRepelAttackResult(attackRoll);
        }

        private bool ResolveRepelAttackResult(AttackRoll attackRoll)
        {
            bool attackRepelled = false;
            if (attackRoll.Result > 0)
                attackRepelled = RepelAttackMoraleCheck(attackRoll);
            else
                Report.Add(attackRoll.Attacker.ToString() + "'s repel attack missed!");

            return attackRepelled;
        }


        // If the attacker passes his morale check, he continues his attack
        // and is hit by the repel weapon used to try to stop him.
        // If attacker fails the morale check, he was stymied and aborts.
        private bool RepelAttackMoraleCheck(AttackRoll attackRoll)
        {
            int checkModifier = attackRoll.Result / 2;
            var moraleCheck = new MoraleCheck(attackRoll.Attacker, MoraleCheckType.RepelAttack,
                attackRoll.Defender, checkModifier);

            if (moraleCheck.IsSuccessful)
            {
                RepelAttackDamageRoll(attackRoll);
                Report.Add(attackRoll.Attacker + " didn't stop the attack, but hit " + attackRoll.Defender + " for " + Damage + " dmg.");
                return false;
            }
            else
            {
                Report.Add(attackRoll.Attacker + " succeeded in repelling the attack.");
                return true;
            }
        }


        private void RepelAttackDamageRoll(AttackRoll attackRoll)
        {
            var damageRoll = new DamageRoll(attackRoll);
            if (damageRoll.Damage > 0)
            {
                Damage = REPEL_ATTACK_DAMAGE;
                Report.Add(attackRoll.Defender + REPEL_DAMAGE_REPORT);
            }
            else
            {
                Damage = 0;
            }
        }
    }
}
