using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using System.Collections.Generic;

namespace Paramita.GameLogic.Actors.Animals
{
    public class GiantRat : Animal, INpc
    {
        public GiantRat() 
            : base(BeingType.GiantRat)
        {
            InitializeAttributes();
            InitializeItemLists();
        }

        protected override void InitializeAttributes()
        {
            name = "Giant Rat";
            hitPoints = 5;
            protection = 2;
            magicResistance = 5;
            strength = 3;
            attackSkill = 8;
            defenseSkill = 10;
            precision = 5;
            morale = 6;
            encumbrance = 1;
            fatigue = 0;
            size = 1;
        }


        protected override void InitializeItemLists()
        {
            base.InitializeItemLists();

            naturalWeapons = new List<Weapon>();
            naturalWeapons.Add(ItemCreator.CreateBite());

            TryToEquipItem(naturalWeapons[0]);

            attacks = new List<Weapon>();
            UpdateAttacks();

            shields = new List<Shield>();

        }

        public void Update(Player player)
        {
            PerformAI(player);
            base.Update();
        }



        // For combat testing, the GiantRat just checks to see if the player
        // is next to it and moves to his tile (which begins an attacks).
        public void PerformAI(Player player)
        {
            Compass direction = Compass.None;
            if(CurrentTile.AdjacentTo(player.CurrentTile, out direction))
            {
                MoveTo(direction);
            }
        }
    }
}
