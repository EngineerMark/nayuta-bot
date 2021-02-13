using System.Collections;
using System.Collections.Generic;

namespace nayuta.Coroutine
{
    public class Yielder
    {
        public static Yielder Instance;
        
        internal List<Coroutine> coroutines = new List<Coroutine>();

        public Yielder()
        {
            Instance = this;
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            Coroutine coroutine = new Coroutine(routine);
            coroutine.routine.MoveNext();
            coroutines.Add(coroutine);
            return coroutine;
        }

        public void ProcessCoroutines()
        {
            for (int i = 0; i < coroutines.Count; i++)
            {
                Coroutine coroutine = coroutines[i];
                if (coroutine.MoveNext())
                    ++i;
                else if (coroutines.Count > 1)
                {
                    coroutines[i] = coroutines[coroutines.Count - 1];
                    coroutines.RemoveAt(coroutines.Count-1);
                }
                else
                {
                    coroutines.Clear();
                    break;
                }
            }
        }
    }
}