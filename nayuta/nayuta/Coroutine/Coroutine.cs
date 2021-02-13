using System.Collections;
using System.Collections.Generic;

namespace nayuta.Coroutine
{
    public class YieldInstruction
    {
        internal IEnumerator routine;

        internal YieldInstruction()
        {
            // Empty
        }

        internal bool MoveNext()
        {
            if (routine.Current is YieldInstruction yieldInstruction)
            {
                if (yieldInstruction.MoveNext())
                    return true;
                else if (routine.MoveNext())
                    return true;
                else
                    return false;
            }
            else if (routine.MoveNext())
                return true;
            else
                return false;
        }
    }

    public class Coroutine : YieldInstruction
    {
        internal Coroutine(IEnumerator routine) => this.routine = routine;
    }
}