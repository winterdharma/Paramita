namespace Paramita.GameLogic.Items.Weapons
{
    public class ShortSword : Weapon
    {
        string description;
        
        public ShortSword() : base(5, 0, 2, 1)
        {
            name = "Short Sword";
            this.description = "A modest weapon to start with.";
            equipType = EquipType.Hand;
        }
    }
}
