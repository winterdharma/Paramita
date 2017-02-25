namespace Paramita.GameLogic.Items.Consumables
{
    public class Meat : Consumable
    {
        private const string NAME = "meat";
        private const string DESCRIPTION = "A chunk of tasty salted meat.";
        private const int SUSTENANCE = 480;

        public int Sustanence
        {
            get { return SUSTENANCE; }
        }

        public Meat() : base(ItemType.Meat)
        {
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
