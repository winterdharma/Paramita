using Paramita.GameLogic.Levels;
using Paramita.GameLogic.Mechanics;
using System;

namespace Paramita.GameLogic.Actors
{

    // This interface defines the methods required for a SentientBeing instance
    // to act as an Non-Player Character in the game.
    // NPCs require:
    //     * an AI routine
    //     * and a way of storing and deciding its intent towards the player.
    public interface INpc
    {
        Tile CurrentTile { get; set; }
        bool IsDead { get; set; }
        Compass Facing { get; }
        BeingType BeingType { get; }
        int TimesAttacked { get; set; }

        event EventHandler<MoveEventArgs> OnMoveAttempt;
        event EventHandler<MoveEventArgs> OnActorDeath;
        event EventHandler<StatusMessageEventArgs> OnStatusMsgSent;

        void PerformAI(Player player);
        void Update(Player player);
    }
}
