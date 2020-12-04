using System;

public class Singleton<T> where T : class
{
    private static readonly object sync = new object();
    private static volatile T instance = null;

    public static T In
    {
        get
        {
            if (instance == null)
            {
                CreateInstance();
            }

            return instance;
        }
    }
    
    static void CreateInstance()
    {
        lock (sync)
        {
            if (instance == null)
            {
                instance = (T)Activator.CreateInstance(typeof(T),true);
            }
        }
    }

    public virtual void Destroy() 
    {
        if (instance != null)
        {
            instance = null;
        }
    }

    public static bool HasInstance => instance != null;
}