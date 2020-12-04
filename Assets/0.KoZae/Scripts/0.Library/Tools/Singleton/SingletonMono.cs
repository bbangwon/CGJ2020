using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : Component
{
    protected static T instance = null;
    public static T In
    {
        get
        {
            if(applicationIsQuitting)
            {
                return null;
            }
            else
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if(instance == null)
                    {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();

                        DontDestroyOnLoad(instance);
                    }
                }

                return instance;
            }
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }

        DoAwake();
    }

    public static bool HasInstance => !IsDestroyed;
    public static bool IsDestroyed => instance == null;

    protected virtual void DoAwake() { }

    protected static bool applicationIsQuitting = false;
    protected virtual void OnApplicationQuit() { applicationIsQuitting = true; }
}