namespace Paramita.GameLogic.Items.Weapons
{
    public class Bite : NaturalWeapon
    {
        public Bite() : base(2, 0, -1, 0, ItemType.Bite, "bite")
        {
            name = "Bite";
            EquipType = EquipType.None;
        }
    }
}
