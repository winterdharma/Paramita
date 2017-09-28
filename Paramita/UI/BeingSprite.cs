using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.GameLogic.Mechanics;
using Paramita.UI.Base;
using Paramita.UI.Scenes;

namespace Paramita.UI
{
    /*
     * This class is responsible for drawing sprites for SentientBeings. It stores
     * multiple textures and determines which to draw according to the SentientBeing's
     * facing direction.
     */
    public class BeingSprite : Sprite
    {
        private Compass facing = Compass.East;
        public Dictionary<Compass, Rectangle> Textures { get; private set; }

        public Compass Facing
        {
            get { return facing; }
            set { facing = value; SetCurrentSprite(); }
        }

        public BeingSprite(Texture2D spritesheet, Rectangle frame) : base(spritesheet, frame)
        {
            Textures = SetTextures();
        }


        private Dictionary<Compass, Rectangle> SetTextures()
        {
            var dictionary = new Dictionary<Compass, Rectangle>();

            dictionary.Add(Compass.East, new Rectangle(0, 0, Frame.Width, Frame.Height));
            dictionary.Add(Compass.West, new Rectangle(32, 0, Frame.Width, Frame.Height));

            return dictionary;
        }

        public override void Update(GameTime gameTime)
        {
            SetCurrentSprite();
        }

        protected void SetCurrentSprite()
        {
            if (facing == Compass.North  || facing == Compass.Northeast 
                || facing == Compass.East || facing == Compass.Southeast)
                facing = Compass.East;
            else if (facing == Compass.South || facing == Compass.Northwest 
                || facing == Compass.West || facing == Compass.Southwest)
                facing = Compass.West;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
                Textures[facing],
                Color.White
                );

            spriteBatch.End();
        }
    }
}
