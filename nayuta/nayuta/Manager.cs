using System;

namespace nayuta
{
    public abstract class Manager<T> where T : class
    {
        private static T sInstance;

        public static T Instance
        {
            get { return sInstance; }
            set { sInstance = value; }
        }

        public Manager()
        {
            Instance = this as T;
        }
    }
}