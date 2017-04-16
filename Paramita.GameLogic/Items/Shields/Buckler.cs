namespace Paramita.GameLogic.Items.Armors
{
    public class Buckler : Shield
    {
        public Buckler() : base(14, 0, 2, 2, ItemType.Shield, "buckler")
        {
            EquipType = EquipType.Hand;
        }
    }
}
