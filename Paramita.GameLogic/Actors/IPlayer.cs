using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;

namespace Paramita.GameLogic.Actors
{
    public interface IPlayer
    {
        Tile CurrentTile { get; set; }
        Compass Facing { get; }
        BeingType BeingType { get; }
        int TimesAttacked { get; set; }

        event EventHandler<MoveEventArgs> OnMoveAttempt;
        event EventHandler<MoveEventArgs> OnActorDeath;
        event EventHandler<LevelChangeEventArgs> OnLevelChange;

        void Update();
    }
}
