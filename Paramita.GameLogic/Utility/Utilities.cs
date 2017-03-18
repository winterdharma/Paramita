using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.GameLogic.Utility
{
    public static class Utilities
    {
        public static void ThrowExceptionIfNull(object o)
        {
            if (o == null)
                throw new NullReferenceException();
        }
    }
}
