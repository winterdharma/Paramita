using Microsoft.Xna.Framework;
//using Paramita.Levels;

namespace Paramita.UI.Base
{
    public static class Camera
    {
        public static Vector2 Position { get;  private set; }

        public static Matrix Transformation
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }

        
        // Keeps the viewport within the bounds of the TileMap so that it stops scrolling when an edge is reached
        public static void LockCamera(Point mapSizeInPixels, Rectangle viewport)
        {
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, mapSizeInPixels.X - viewport.Width),
                MathHelper.Clamp(Position.Y, 0, mapSizeInPixels.Y - viewport.Height)
            );
        }


        // Sets the Camera's position so that the viewport is centered on @position, then checks to see if the
        // viewport is outside of the TileMap's bounds
        public static void LockToSprite(Point mapSizeInPixels, Vector2 position, Rectangle viewport)
        {
            Position = new Vector2(
                (position.X + 16) - (viewport.Width / 2),
                (position.Y + 16) - (viewport.Height / 2)
            );
            LockCamera(mapSizeInPixels, viewport);
        }
    }
}
