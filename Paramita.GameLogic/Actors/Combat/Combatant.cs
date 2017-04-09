using Paramita.GameLogic.Actors.Combat;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramita.GameLogic.Actors
{
    public class AttackEventArgs : EventArgs
    {
        public List<string> Report { get; set; }
        
        public AttackEventArgs(List<string> report)
        {
            Report = report;
        }
    }

    public class Combatant
    {
        #region Fields
        private string _name;
        private int _hitPoints;
        private int _protection;
        private int _magicResistance;
        private int _strength;
        private int _morale;
        private int _attackSkill;
        private int _defenseSkill;
        private int _precision;
        private int _encumbrance;
        private int _totalEncumbrance;
        private int _fatigue;
        private int _size;
        private int _timesAttacked;

        private RepelMeleeAttack _repelMeleeAttack;
        private MeleeAttack _meleeAttack;

        private List<Attack> _attacks;
        private List<Shield> _shields;
        #endregion


        #region Events
        public event EventHandler<AttackEventArgs> OnAttackResolved;
        public event EventHandler<EventArgs> OnCombatantKilled;
        #endregion


        #region Constructors
        public Combatant(List<int> data) :
            this(data[0], data[1], data[2], data[3], data[4], data[5],
                data[6], data[7], data[8], data[9], new List<Weapon>(), new List<Shield>())
        { }

        public Combatant(int hitpoints, int protection, int magicResistance, int strength, int morale,
            int attackskill, int defenseskill, int precision, int encumbrance, int size, List<Weapon> weapons, List<Shield> shields)
        {
            _hitPoints = hitpoints;
            _protection = protection;
            _magicResistance = magicResistance;
            _strength = strength;
            _morale = morale;
            _attackSkill = attackskill;
            _defenseSkill = defenseskill;
            _precision = precision;
            _encumbrance = encumbrance;
            _size = size;
            _shields = shields;

            _attacks = new List<Attack>();

            _fatigue = 0;

            _repelMeleeAttack = new RepelMeleeAttack();
            _meleeAttack = new MeleeAttack();
        }
        #endregion


        #region Properties
        public string Name
        {
            set { _name = value; }
        }

        public List<Shield> Shields
        {
            set {_shields = value; }
        }

        public int HitPoints {
            get { return _hitPoints; }
            set
            {
                _hitPoints = value;
                if (_hitPoints < 1)
                {
                    OnCombatantKilled?.Invoke(this, EventArgs.Empty);
                }
            } }

        public int Protection
        {
            get { return _protection; }
        }

        public int ShieldProtection
        {
            get { return _shields.Sum(shield => shield.Protection); }
        }

        public int Strength
        {
            get { return _strength; }
        }

        public int Morale
        {
            get { return _morale; }
        }

        public int AttackSkill
        {
            get { return _attackSkill; }
        }

        public int DefenseSkill
        {
            get { return _defenseSkill; }
        }

        public int TotalDefense
        {
            get
            {
                int modifier = _attacks.Sum(attack => attack.Weapon.DefenseModifier);
                return _defenseSkill + modifier;
            }
        }

        public int Parry
        {
            get { return _shields.Sum(shield => shield.Parry); }
        }

        public int Encumbrance
        {
            get { return _encumbrance; }
        }

        public int TotalEncumbrance
        {
            set { _totalEncumbrance = value; }
        }

        public int Fatigue
        {
            get { return _fatigue; }
            set { _fatigue = value; }
        }

        public int FatigueAttPenalty
        {
            get { return _fatigue / 20; }
        }

        public int FatigueDefPenalty
        {
            get { return _fatigue / 10; }
        }

        public int FatigueCriticalPenalty
        {
            get { return _fatigue / 15; }
        }

        public int Size
        {
            get { return _size; }
        }

        public int TimesAttacked
        {
            get { return _timesAttacked; }
            set { _timesAttacked = value; }
        }
        #endregion


        // will be called by Actor when handling Inventory change events
        public void UpdateAttacks(List<Weapon> weapons)
        {
            var attacks = new List<Attack>();
            foreach(var weapon in weapons)
            {
                attacks.Add(new Attack(weapon, _strength, _attackSkill));
            }
            _attacks = attacks;
        }

        // conduct all of a being's attacks for the turn
        public void Attack(Combatant defender)
        {
            //GameScene.PostNewStatus(this + " attacked " + defender + ".");
            foreach (var attack in _attacks)
            {
                var result = new List<string>() { this + " attacks " + defender + " with " + attack.Weapon.DisplayText() + "." };
                result.AddRange(ConductAttack(attack, defender));
                OnAttackResolved?.Invoke(this, new AttackEventArgs(result));
                if (_hitPoints < 1 || defender.HitPoints < 1)
                    break;
            }
        }

        private List<string> ConductAttack(Attack attack, Combatant defender)
        {
            var report = new List<string>();
            Tuple<bool,bool, List<string>> repelResult;

            repelResult = ConductRepelAttack(attack, defender);
            report.AddRange(repelResult.Item3);

            if(!repelResult.Item1)
            {
                _meleeAttack.Execute(this, attack, defender);
                report.AddRange(_meleeAttack.Report);
            }
            else if(!repelResult.Item2)
            {
                TakeDamage(_repelMeleeAttack.Damage);

                if (!Dead())
                {
                    _meleeAttack.Execute(this, attack, defender);
                    report.AddRange(_meleeAttack.Report);
                }
                else
                    report.Add(this + " was killed!");
            }
            // if repelResult.Item2 was true, then simply return the report
            return report;
        }

        private Tuple<bool,bool, List<string>> ConductRepelAttack(Attack attack, Combatant defender)
        {
            var report = new List<string>();
            bool repelPossible;
            bool repelSucceeded;

            if (_repelMeleeAttack.Possible(attack, defender))
            {
                repelPossible = true;
                report.AddRange(_repelMeleeAttack.Report);
                repelSucceeded = _repelMeleeAttack.Execute(defender, attack, this);
                report.AddRange(_repelMeleeAttack.Report);
            }
            else
            {
                repelPossible = false;
                report.AddRange(_repelMeleeAttack.Report);
                repelSucceeded = false;
            }

            return new Tuple<bool, bool, List<string>>(repelPossible, repelSucceeded, report);
        }

        // used to conduct repel attacks
        public Weapon GetLongestWeapon()
        {
            if (_attacks.Count > 1)
                return _attacks.OrderBy(attack => attack.Weapon.Length).Last().Weapon;
            else
                return null;
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0)
                HitPoints -= damage;
        }

        public bool Dead()
        {
            return _hitPoints < 1;
        }

        public void IncrementTimesAttacked()
        {
            _timesAttacked++;
        }

        public void AddEncumbranceToFatigue()
        {
            _fatigue += _totalEncumbrance;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
