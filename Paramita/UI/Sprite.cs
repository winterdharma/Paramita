using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Actors;
using Paramita.GameLogic.Items;
using Paramita.GameLogic.Levels;
using Paramita.UI.Base;
using Paramita.UI.Scenes;
using System;

namespace Paramita.UI

{
    /*
     * This class is responsible for storing and drawing sprites that have only
     * one frame to display at all time. It stores the texture, size of the frame,
     * and the pixel coordinates that it is to be drawn to on the screen.
     */

    public enum SpriteType
    {
        None,
        Tile_Floor,
        Tile_Wall,
        Tile_Door,
        Tile_StairsUp,
        Tile_StairsDown,
        Actor_GiantRat,
        Actor_Player,
        Item_Buckler,
        Item_ShortSword,
        Item_Coins,
        Item_Meat
    }

    public class Sprite
    {
        protected Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture { get; set; }

        public Rectangle Frame { get; }

        public Sprite(Texture2D spritesheet, Rectangle frame)
        {
            Texture = spritesheet;
            Frame = frame;
        }


        public static SpriteType GetSpriteType(TileType type)
        {
            switch (type)
            {
                case TileType.Door:
                    return SpriteType.Tile_Door;
                case TileType.Floor:
                    return SpriteType.Tile_Floor;
                case TileType.StairsDown:
                    return SpriteType.Tile_StairsDown;
                case TileType.StairsUp:
                    return SpriteType.Tile_StairsUp;
                case TileType.Wall:
                    return SpriteType.Tile_Wall;
                default:
                    throw new NotImplementedException("Sprite.GetSpriteType(): TileType not implemented.");
            }
        }

        public static SpriteType GetSpriteType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Coins:
                    return SpriteType.Item_Coins;
                case ItemType.Meat:
                    return SpriteType.Item_Meat;
                case ItemType.Shield:
                    return SpriteType.Item_Buckler;
                case ItemType.ShortSword:
                    return SpriteType.Item_ShortSword;
                case ItemType.Bite:
                case ItemType.Fist:
                case ItemType.None:
                    return SpriteType.None;
                default:
                    throw new NotImplementedException("Sprite.GetSpriteType(): ItemType not implemented.");
            }
        }

        public static SpriteType GetSpriteType(ActorType type)
        {
            switch (type)
            {
                case ActorType.GiantRat:
                    return SpriteType.Actor_GiantRat;
                case ActorType.HumanPlayer:
                    return SpriteType.Actor_Player;
                default:
                    throw new NotImplementedException("Sprite.GetSpriteType(): BeingType not implemented.");
            }
        }


        public virtual void Update(GameTime gameTime)
        {
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null,
                Camera.Transformation);

            spriteBatch.Draw(
                Texture,
                position,
                Frame,
                Color.White
                );

            spriteBatch.End();
        }


        public void LockToMap(Point mapSize)
        {
            position.X = MathHelper.Clamp(position.X, 0, mapSize.X - Frame.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, mapSize.Y - Frame.Height);
        }
    }
}
