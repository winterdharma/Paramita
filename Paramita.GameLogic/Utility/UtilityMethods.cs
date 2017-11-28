using System;

namespace Paramita.GameLogic.Utility
{
    public static class UtilityMethods
    {
        public static void ThrowExceptionIfNull(object o)
        {
            if (o == null)
                throw new NullReferenceException();
        }
    }
}
