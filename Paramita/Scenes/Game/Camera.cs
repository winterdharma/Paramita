using Microsoft.Xna.Framework;

namespace Paramita.Scenes
{
    public class Camera
    {
        Vector2 position;
        float speed;
        float zoom;


        public Vector2 Position
        {
            get { return position; }  set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(speed, 1f, 16f); }
        }

        public float Zoom
        {
            get { return zoom; } set { zoom = value; }
        }

        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        // Construct a new Camera class with standard zoom (no scaling)
        public Camera()
        {
            zoom = 1.0f;
            speed = 4f;
        }

        public Camera(Vector2 pos)
        {
            zoom = 1.0f;
            speed = 4f;
            Position = pos;
        }
        


        // Call this method with negative values to zoom out
        // or positive values to zoom in. It looks at the current zoom
        // and adjusts it by the specified amount. If we were at a 1.0f
        // zoom level and specified -0.5f amount it would leave us with
        // 1.0f - 0.5f = 0.5f so everything would be drawn at half size.
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        public void LockCamera(TileMap map, Rectangle viewport)
        {
            position.X = MathHelper.Clamp(position.X, 0, map.WidthInPixels - viewport.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, map.HeightInPixels - viewport.Height);
        }

        public void LockToSprite(TileMap map, AnimatedSprite sprite, Rectangle viewport)
        {
            position.X = (sprite.Position.X + sprite.Width / 2)
            - (viewport.Width / 2);
            position.Y = (sprite.Position.Y + sprite.Height / 2)
            - (viewport.Height / 2);
            LockCamera(map, viewport);
        }
    }
}
