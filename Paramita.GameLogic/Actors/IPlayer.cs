﻿using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using Paramita.GameLogic.Utility;
using System;

namespace Paramita.GameLogic.Actors
{
    public interface IPlayer
    {
        Tile CurrentTile { get; set; }
        Compass Facing { get; }
        ActorType ActorType { get; }
        int TimesAttacked { get; set; }

        event EventHandler<DirectionEventArgs> OnMoveAttempt;
        event EventHandler<LevelChangeEventArgs> OnLevelChange;

        void Update();
    }
}
