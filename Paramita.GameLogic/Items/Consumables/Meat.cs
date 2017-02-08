namespace Paramita.GameLogic.Items.Consumables
{
    public class Meat : Consumable
    {
        private string description;
        private int sustenance;

        public int Sustanence
        {
            get { return sustenance; }
        }

        public Meat() : base()
        {
            name = "Salted Meat";
            this.description = "A chunk of tasty salted meat.";
            sustenance = 480;
        }



        public override string ToString()
        {
            return "a chunk of " + name;
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}
