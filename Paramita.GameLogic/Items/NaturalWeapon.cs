namespace Paramita.GameLogic.Items
{
    /*
     * This is the base class for weapons that are naturally part of beings.
     * For example, bites, punches, kicks, claws, and any other form of attack by
     * beings with parts of their own bodies or other natural abilities are
     * represented as NaturalWeapon instances for combat purposes.
     * 
     * NaturalWeapons have two basic types:
     *      * Bonus attacks that beings get regardless of equiped items
     *      * Default attacks that are replaced by equiped items
     * 
     * NaturalWeapons with an EquipType are default weapons when no weapon is
     * equiped for slots using that EquipType. Example: Humans get a Fist attack
     * if they have no weapon equiped in either hand.
     * 
     * NaturalWeapons with an EquipType of None are bonus attacks that the being
     * gets in addition to any other equiped weapons.
     */
    public class NaturalWeapon : Weapon
    {
        public NaturalWeapon(int damage, int attack, int defense, int length, int encumbrance, ItemType type, string name) 
            : base(damage, attack, defense, length, encumbrance, type, name)
        {
        }
    }
}
