namespace Paramita.GameLogic.Items.Consumables
{
    public class Meat : Consumable
    {
        private const string DESCRIPTION = "A chunk of tasty salted meat.";
        private const int SUSTENANCE = 480;

        public int Sustanence
        {
            get { return SUSTENANCE; }
        }

        public Meat() : base(ItemType.Meat, "Meat")
        {
        }

        public override string GetDescription()
        {
            return DESCRIPTION;
        }
    }
}
