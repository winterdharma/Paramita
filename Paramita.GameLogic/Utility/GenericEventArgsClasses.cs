using Microsoft.Xna.Framework;
using System;

namespace Paramita.GameLogic.Utility
{
    public class MoveEventArgs : EventArgs
    {
        public Point Origin { get; }
        public Point Destination { get; }

        public MoveEventArgs(Point tileOrigin, Point tileDest)
        {
            Origin = tileOrigin;
            Destination = tileDest;
        }
    }

    public class PointEventArgs : EventArgs
    {
        public Point Point { get; }
        public PointEventArgs(Point point)
        {
            Point = point;
        }
    }

    public class RectangleEventArgs : EventArgs
    {
        public Rectangle Rectangle { get; }
        public RectangleEventArgs(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
