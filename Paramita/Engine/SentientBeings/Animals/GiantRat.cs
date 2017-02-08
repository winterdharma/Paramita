﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.Items;
using Paramita.Levels;
using Paramita.Scenes;
using System.Collections.Generic;

namespace Paramita.SentientBeings.Animals
{
    public class GiantRat : Animal, INpc
    {
        public GiantRat(Level level, Texture2D sprites) 
            : base(level, sprites)
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

        public override void Update(GameTime gameTime)
        {

            PerformAI();
            base.Update(gameTime);
        }



        // For combat testing, the GiantRat just checks to see if the player
        // is next to it and attacks if true.
        public void PerformAI()
        {
            Tile playerTile = level.Player.CurrentTile;
            if(CurrentTile.AdjacentTo(playerTile) == true)
            {
                Attack(level.Player);
            }
        }
    }
}