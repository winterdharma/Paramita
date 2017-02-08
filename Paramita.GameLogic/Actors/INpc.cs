namespace Paramita.GameLogic.Actors
{

    // This interface defines the methods required for a SentientBeing instance
    // to act as an Non-Player Character in the game.
    // NPCs require:
    //     * an AI routine
    //     * and a way of storing and deciding its intent towards the player.
    public interface INpc
    {
        /*
         * This method will be called by any relevant Update() method to cause
         * the SentientBeing instance to update itself (move, attack, etc).
         */
        void PerformAI(Player player);
        void Update(Player player);
    }
}
