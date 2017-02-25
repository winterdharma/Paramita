namespace Paramita.GameLogic.Items.Armors
{
    public class Buckler : Shield
    {
        private const string NAME = "buckler";
        private const string DESCRIPTION = "A small shield for parrying attacks.";


        public Buckler() : base(14, 0, 2, 0, ItemType.Shield)
        {
            EquipType = EquipType.Hand;
        }


        public override string ToString()
        {
            return NAME;
        }

        public override string GetDescription()
        {
            return DESCRIPTION;
        }
    }
}
