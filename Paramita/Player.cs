using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita
{
    public class Player : Sattva
    {
        public void Draw( SpriteBatch spriteBatch )
        {
            spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Height),
                null, null, null, 0.0f, Vector2.One, Color.White, 
                SpriteEffects.None, LayerDepth.Sprites); 
        }

        public bool HandleInput(InputState input, IMap map)
        {
            if (input.IsLeft(PlayerIndex.One))
            {
                int tempX = X - 1;
                if (map.IsWalkable(tempX, Y))
                {
                    Enemy e = Global.CombatManager.EnemyAt(tempX, Y);
                    if (e == null)
                    {
                        X = tempX;
                    }
                    else
                    {
                        Global.CombatManager.Attack(this, e);
                    }
                    return true;
                }
            }
            else if (input.IsRight(PlayerIndex.One))
            {
                int tempX = X + 1;
                {
                    Enemy e = Global.CombatManager.EnemyAt(tempX, Y);
                    if (e == null)
                    {
                        X = tempX;
                    }
                    else
                    {
                        Global.CombatManager.Attack(this, e);
                    }
                    return true;
                }
            }
            else if (input.IsUp(PlayerIndex.One))
            {
                int tempY = Y - 1;
                {
                    Enemy e = Global.CombatManager.EnemyAt(X, tempY);
                    if (e == null)
                    {
                        Y = tempY;
                    }
                    else
                    {
                        Global.CombatManager.Attack(this, e);
                    }
                    return true;
                }
            }
            else if (input.IsDown(PlayerIndex.One))
            {
                int tempY = Y + 1;
                {
                    Enemy e = Global.CombatManager.EnemyAt(X, tempY);
                    if (e == null)
                    {
                        Y = tempY;
                    }
                    else
                    {
                        Global.CombatManager.Attack(this, e);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
