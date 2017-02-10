namespace Paramita.GameLogic.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string description;
        
        public ShortSword() : base(5, 0, 2, 1, ItemType.ShortSword)
        {
            name = "Short Sword";
            this.description = "A modest weapon to start with.";
            EquipType = EquipType.Hand;
        }
    }
}
