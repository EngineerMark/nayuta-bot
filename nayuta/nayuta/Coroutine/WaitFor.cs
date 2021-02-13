using System;
using System.Collections;

namespace nayuta.Coroutine
{
    public static class WaitFor
    {
        public static IEnumerator WaitForSeconds(float seconds)
        {
            yield return Yielder.Instance.StartCoroutine(_waitForSeconds(seconds));
        }

        private static IEnumerator _waitForSeconds(float seconds)
        {
            float startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            while (DateTimeOffset.Now.ToUnixTimeSeconds() - startTime < seconds)
                yield return null;
        }
    }
}