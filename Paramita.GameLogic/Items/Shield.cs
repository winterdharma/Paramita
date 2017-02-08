namespace Paramita.GameLogic.Items
{
    public class Shield : Item
    {
        protected int encumbrance;
        protected int parry;
        protected int defenseModifier;
        protected int protection;

        public int Encumbrance { get { return encumbrance; } }
        public int Parry { get { return parry; } }
        public int DefenseModifier { get { return defenseModifier; } }
        public int Protection { get { return protection; } }


        public Shield(int prot, int defMod, int parry, int enc) : base()
        {
            encumbrance = enc;
            this.parry = parry;
            defenseModifier = defMod;
            protection = prot;
        }


        public override string ToString()
        {
            return name;
        }

        public override string GetDescription()
        {
            return name + "(Prot: " + protection + ", DefenseMod: " + defenseModifier 
                + ", Parry: " + parry + ", Enc: " + encumbrance + ")";
        }
    }
}
