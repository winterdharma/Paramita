namespace Paramita.GameLogic.Items.Weapons
{
    public class ShortSword : Weapon
    {
        public ShortSword() : base(5, 0, 2, 1, 2, ItemType.ShortSword, "short sword")
        {
            EquipType = EquipType.Hand;
        }
    }
}
