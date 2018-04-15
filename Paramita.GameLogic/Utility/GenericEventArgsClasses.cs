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
}
