namespace Paramita.GameLogic.Items
{
    /*
     * Armors are items used to protect beings from harm, usually from combat damage.
     * Includes protection from magic or the elements as well.
     * 
     * This is an abstract class not intended to be instantiated.
     */
    public abstract class Armor : Item
    {
        #region Fields
        private int _encumbrance;
        private int _protection;
        private int _defenseModifier;
        #endregion

       
        public Armor(int prot, int defMod, int enc, ItemType type, string name) : base(type, name)
        {
            Encumbrance = enc;
            _protection = prot;
            _defenseModifier = defMod;
        }


        #region Properties
        public int DefenseModifier { get { return _defenseModifier; } }
        public int Protection { get { return _protection; } }
        #endregion


        public override string GetDescription()
        {
            return _name + " (Protection: " + _protection + ", Defense modifier: " 
                + _defenseModifier + ", Encumbrance: " + _encumbrance + ")";
        }
    }
}
