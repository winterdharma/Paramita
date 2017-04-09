﻿namespace Paramita.GameLogic.Items
{
    public class Shield : Item
    {
        #region Fields
        protected int _encumbrance;
        protected int _parry;
        protected int _defenseModifier;
        protected int _protection;
        #endregion


        public Shield(int prot, int defMod, int parry, int enc, ItemType type, string name) : base(type, name)
        {
            _encumbrance = enc;
            this._parry = parry;
            _defenseModifier = defMod;
            _protection = prot;
        }


        #region Properties
        public int Encumbrance { get { return _encumbrance; } }
        public int Parry { get { return _parry; } }
        public int DefenseModifier { get { return _defenseModifier; } }
        public int Protection { get { return _protection; } }
        #endregion


        public override string GetDescription()
        {
            return name + "(Prot: " + _protection + ", DefenseMod: " + _defenseModifier 
                + ", Parry: " + _parry + ", Enc: " + _encumbrance + ")";
        }
    }
}
