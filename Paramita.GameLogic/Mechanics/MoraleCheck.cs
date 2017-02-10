using Paramita.GameLogic.Actors;

namespace Paramita.GameLogic.Mechanics
{
    /*
     * A morale check is a way to check if a being will decide not to proceed with
     * an action because his ability to overcome dread or fear fails.
     * 
     * A morale check proceeds like an AttackRoll except that the checker needs
     * to score better than a certain threshold that expresses the level of danger
     * he faces.
     */

    public enum MoraleCheckType
    {
        RepelAttack
    }
    
    public class MoraleCheck
    {
        private Dice dice2d6;
        private int checkerMorale;
        private int modifier;
        private int threshold;
        private int sizeDifference;

        private bool isSuccessful;

        public bool IsSuccessful
        {
            get { return isSuccessful; }
        }




        public MoraleCheck(Actor checker, MoraleCheckType type, Actor opponent = null, int modifier = 0)
        {
            dice2d6 = new Dice(2);
            checkerMorale = checker.Morale;
            this.modifier = modifier;
            threshold = GetThreshold(type);
            sizeDifference = GetSizeDifference(checker, opponent);
            isSuccessful = ResolveMoraleCheck();
        }




        private int GetThreshold(MoraleCheckType type)
        {
            if(type == MoraleCheckType.RepelAttack)
            {
                return 10;
            }
            return 0;
        }


        private int GetSizeDifference(Actor checker, Actor opponent)
        {
            if(opponent != null)
                return checker.Size - opponent.Size;
            return 0;
        }


        private bool ResolveMoraleCheck()
        {
            int checkerRoll = dice2d6.OpenEndedDiceRoll();
            int checkAgainstRoll = dice2d6.OpenEndedDiceRoll();

            int checkerTotal = checkerRoll + checkerMorale + sizeDifference;
            int checkAgainstTotal = checkAgainstRoll + threshold + modifier;

            if (checkerTotal > checkAgainstTotal)
                return true;

            return false;
        }
    }
}
