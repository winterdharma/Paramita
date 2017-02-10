using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Paramita.UI.Scenes;


namespace Paramita.UI

{
    /*
     * This class is responsible for storing and drawing sprites that have only
     * one frame to display at all time. It stores the texture, size of the frame,
     * and the pixel coordinates that it is to be drawn to on the screen.
     */
    public class Sprite
    {
        protected Rectangle frame;
        protected Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture { get; set; }


        public Sprite(Texture2D spritesheet, Rectangle frame)
        {
            Texture = spritesheet;
            this.frame = frame;
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
                frame,
                Color.White
                );

            spriteBatch.End();
        }


        public void LockToMap(Point mapSize)
        {
            position.X = MathHelper.Clamp(position.X, 0, mapSize.X - frame.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, mapSize.Y - frame.Height);
        }
    }
}
