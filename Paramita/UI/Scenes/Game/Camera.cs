﻿using Microsoft.Xna.Framework;
using Paramita.Levels;

namespace Paramita.Scenes
{
    public static class Camera
    {
        public static Vector2 Position { get;  set; }


        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public static Matrix Transformation
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }

        

        // Keeps the Camera's position within the bounds of the viewPort
        public static void LockCamera(TileMap map, Rectangle viewport)
        {
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, map.MapSizeInPixels.X - viewport.Width),
                MathHelper.Clamp(Position.Y, 0, map.MapSizeInPixels.Y - viewport.Height)
            );
        }

        // Sets the Camera's position centered on the Sprite, except when it would move the viewPort's bounds
        // would move outside the bounds of the TileMap.
        public static void LockToSprite(TileMap map, Vector2 position, Rectangle viewport)
        {
            Position = new Vector2(
                (position.X + 16) - (viewport.Width / 2),
                (position.Y + 16) - (viewport.Height / 2)
            );
            LockCamera(map, viewport);
        }
    }
}