namespace Paramita.GameLogic.Items.Armors
{
    public class Buckler : Shield
    {
        private string description;


        public Buckler() : base(14, 0, 2, 0)
        {
            name = "Buckler";
            this.description = "A small shield for parrying attacks.";
            equipType = EquipType.Hand;
        }


        public override string ToString()
        {
            return name;
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}
