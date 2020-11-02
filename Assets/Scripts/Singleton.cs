using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> //prevent arbitrary objects from being defined as singletons
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
        set { }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate seconde singleton class");
            Destroy(this);
            return;
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }


}