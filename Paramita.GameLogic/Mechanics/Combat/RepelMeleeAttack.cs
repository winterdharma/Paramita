using Paramita.GameLogic.Items;
using Paramita.GameLogic.Actors;
using System.Collections.Generic;

namespace Paramita.GameLogic.Mechanics
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
    class RepelMeleeAttack
    {
        private Actor attacker;
        private Actor defender;
        private Weapon attackWeapon;
        private Weapon repelWeapon;

        private const int REPEL_ATTACK_DAMAGE = 1;
        private readonly string REPEL_DAMAGE_REPORT = " takes " + REPEL_ATTACK_DAMAGE + " pt of damage!";
        private const string ATTACKER_KILLED = " is killed!";

        private bool isSuccessful;
        private int damage;

        private List<string> _repelAttackLog = new List<string>();

        public bool IsSuccessful
        {
            get { return isSuccessful; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public List<string> RepelAttackLog { get { return _repelAttackLog; } }


        public RepelMeleeAttack(Attack attack)
        {
            attacker = attack.Attacker;
            defender = attack.Defender;
            attackWeapon = attack.Weapon;
            repelWeapon = defender.GetLongestWeapon();

            isSuccessful = ResolveRepelAttack();
        }



        
        private bool ResolveRepelAttack()
        {
            if(CheckForRepelAttempt())
            {
                var attackRoll = new AttackRoll(defender, repelWeapon, attacker);

                _repelAttackLog.AddRange( 
                    new string[] { attackRoll.AttackerReport, attackRoll.DefenderReport } );

                return ResolveRepelAttackResult(attackRoll);
            }
            return false;   
        }


        private bool CheckForRepelAttempt()
        {
            if (repelWeapon.Length > attackWeapon.Length)
                return true;
            return false;
        }


        private bool ResolveRepelAttackResult(AttackRoll attackRoll)
        {
            bool attackRepelled = false;
            if (attackRoll.Result > 0)
                attackRepelled = RepelAttackMoraleCheck(attackRoll);

            if (CheckIfRepelAttackKilledAttacker())
                attackRepelled = true;

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
                return false;
            }
            else
                return true;
        }


        private void RepelAttackDamageRoll(AttackRoll attackRoll)
        {
            var damageRoll = new DamageRoll(attackRoll);
            if (damageRoll.Damage > 0)
            {
                damage = REPEL_ATTACK_DAMAGE;
                _repelAttackLog.Add(attacker + REPEL_DAMAGE_REPORT);
            }
        }


        private bool CheckIfRepelAttackKilledAttacker()
        {
            if (attacker.IsDead)
            {
                _repelAttackLog.Add(attacker + ATTACKER_KILLED);
                //GameScene.PostNewStatus();
                return true;
            }
            return false;
        }
    }
}
