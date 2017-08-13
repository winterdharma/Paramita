using Microsoft.Xna.Framework;
using Paramita.GameLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Utility
{
    public class IntegerEventArgs : EventArgs
    {
        public int Value { get; }

        public IntegerEventArgs(int value)
        {
            Value = value;
        }
    }

    public class DirectionEventArgs : EventArgs
    {
        public Compass Direction { get; }

        public DirectionEventArgs(Compass direction)
        {
            Direction = direction;
        }
    }

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
