namespace Paramita.GameLogic.Items.Weapons
{
    public class Bite : NaturalWeapon
    {
        public Bite() 
            : base(2, 0, -1, 0)
        {
            name = "Bite";
            equipType = EquipType.None;
        }
    }
}
