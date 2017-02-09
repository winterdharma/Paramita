namespace Paramita.GameLogic.Items.Weapons
{
    public class Fist : NaturalWeapon
    {
        public Fist() : base(-2, -1, -1, 0, ItemType.Fist)
        {
            name = "Fist";
            EquipType = EquipType.Hand;
        }
    }
}
