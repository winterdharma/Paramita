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
            : base(ActorType.GiantRat, new List<int>() { 5, 2, 5, 3, 6, 8, 10, 5, 1, 1 } )
        {
            SubscribeToEvents();
            Name = "Giant Rat";
            InitializeItemLists();
        }

        
        private void InitializeItemLists()
        {
            Inventory.NaturalWeapons.Add(ItemCreator.CreateBite());
            Inventory.AddToWeapons(Inventory.NaturalWeapons);
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
