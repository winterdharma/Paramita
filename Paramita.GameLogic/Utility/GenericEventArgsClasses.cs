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
}
