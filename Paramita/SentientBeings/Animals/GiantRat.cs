using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Scenes;
using System.Collections.Generic;
using System;
using Paramita.Mechanics;

namespace Paramita.SentientBeings.Animals
{
    public class GiantRat : Animal, INpc
    {
        public GiantRat(GameScene gameScene, Texture2D sprites, Rectangle rightFacing, Rectangle leftFacing) 
            : base(gameScene, sprites, rightFacing, leftFacing)
        {
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

            naturalWeapons = new List<Weapon>();
            naturalWeapons.Add(ItemCreator.CreateBite());

            attacks = new List<Weapon>();
            attacks.AddRange(naturalWeapons);
        }


        public override void Update(GameTime gameTime)
        {
            PerformAI();
            base.Update(gameTime);
        }

        public void PerformAI()
        {
            MoveTo(Compass.East);
        }


        private void MoveTo(Compass direction)
        {
            facing = direction;
            SetCurrentSprite();
            Tile newTile = gameScene.Map.GetTile(currentTile.TilePoint + Direction.GetPoint(Facing));

            if(newTile!=null && newTile.IsWalkable == true)
            {
                currentTile = newTile;
            }
        }
    }
}
