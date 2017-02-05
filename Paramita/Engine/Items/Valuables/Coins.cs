using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Paramita.Items.Valuables
{
    // This enum may be implemented in the future
    //public enum CoinTypes
    //{
    //    platinum = 1,
    //    gold = 10,
    //    silver = 100,
    //    copper = 1000
    //}

    public class Coins : Valuable
    {
        private int number;

        public int Number { get { return number; } }



        public Coins(Texture2D texture, int number) 
            : base(texture)
        {
            name = "Gold Coin";
            this.number = number;
        }



        public void AddCoins(int coinsToAdd)
        {
            number += coinsToAdd;
        }



        public void SubtractCoins(int coinsToSubtract)
        {
            if (number > coinsToSubtract)
            {
                number -= coinsToSubtract;
            }
        }



        public override string ToString()
        {
            if(number == 1)
                return number + " " + name;

            return number + " " + name + "s";
        }

        public override string GetDescription()
        {
            return number + " " + name;
        }
    }
}
