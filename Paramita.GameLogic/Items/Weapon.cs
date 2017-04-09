namespace Paramita.GameLogic.Items
{
    /*
     * Weapons are used to engage in combat or otherwise harm living things.
     * This is a base class not intended to be directly instantiated.
     */
    public abstract class Weapon : Item
    {
        #region Fields
        protected int _damage;
        protected int _attackModifier;
        protected int _defenseModifier;
        protected int _length;
        protected int _encumbrance;
        #endregion


        public Weapon(int damage, int attack, int defense, int length, int encumbrance, ItemType type, string name) : base(type, name)
        {
            _damage = damage;
            _attackModifier = attack;
            _defenseModifier = defense;
            _length = length;
            _encumbrance = encumbrance;
        }


        #region Properties
        public int Damage { get { return _damage; } }

        public int AttackModifier { get { return _attackModifier; } }

        public int DefenseModifier { get { return _defenseModifier; } }

        public int Length { get { return _length; } }

        public int Encumbrance { get { return _encumbrance; } }
        #endregion


        public override string GetDescription()
        {
            return name + " (Damage: " + _damage + " plus strength, Att:" + _attackModifier 
                + ", Def:" + _defenseModifier + ", Length:" + _length + ", Encumbrance: " 
                + _encumbrance + ")";
        }
    }
}
