using System;
using System.Collections;
using System.Collections.Generic;

namespace nayuta.Internal
{
    // Kind of a wrapper for List, making it possible to add functionality to it
    [Serializable]
    public class BetterList<T> : List<T>
    {
        //Returns a random element from the list
        public T Random()
        {
            if (Count == 0)
                return default(T);

            return this[new Random((int)DateTimeOffset.Now.ToUnixTimeSeconds()).Next(0, Count)];
        }
    }
}
