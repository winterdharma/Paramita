namespace Paramita.GameLogic.Items.Weapons
{
    public class ShortSword : Weapon
    {
        private const string DESCRIPTION = "A modest weapon to start with.";

        public ShortSword() : base(5, 0, 2, 1, ItemType.ShortSword, "short_sword")
        {
            EquipType = EquipType.Hand;
        }

        public override string GetDescription()
        {
            return DESCRIPTION;
        }
    }
}
