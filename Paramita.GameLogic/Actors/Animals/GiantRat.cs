using Microsoft.Xna.Framework;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Mechanics;
using System;
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
            var direction = Compass.None;
            if (CurrentTile.AdjacentTo(player.CurrentTile, out direction))
            {
                MoveTo(direction);
                return;
            }

            var presentLoc = new Vector2(CurrentTile.TilePoint.X, CurrentTile.TilePoint.Y);
            var playerLoc = new Vector2(player.CurrentTile.TilePoint.X, player.CurrentTile.TilePoint.Y);
            var distanceFromPlayer = Vector2.Distance(presentLoc, playerLoc);

            if(distanceFromPlayer <= 3)
            {
                MoveTo(GetDirectionTowardsDestination(presentLoc, playerLoc));
            }
        }

        private Compass GetDirectionTowardsDestination(Vector2 startLocation, Vector2 destination)
        {
            var direction = Compass.None;
            Tuple<Vector2, Compass>[] locations = new Tuple<Vector2,Compass>[] {
            new Tuple<Vector2, Compass>(new Vector2(startLocation.X, startLocation.Y - 1), Compass.North),
            new Tuple<Vector2, Compass>(new Vector2(startLocation.X, startLocation.Y + 1), Compass.South),
            new Tuple<Vector2, Compass>(new Vector2(startLocation.X + 1, startLocation.Y), Compass.East),
            new Tuple<Vector2, Compass>(new Vector2(startLocation.X - 1, startLocation.Y), Compass.West)
            };

            float minDistance = Vector2.Distance(locations[0].Item1, destination);
            direction = locations[0].Item2;
            for(int i = 1; i < locations.Length; i++)
            {
                var distance = Vector2.Distance(locations[i].Item1, destination);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    direction = locations[i].Item2;
                }
            }

            return direction;
        }
    }
}
