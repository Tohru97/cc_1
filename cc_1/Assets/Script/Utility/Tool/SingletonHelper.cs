using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object lockObject = new object();
    private static bool isQuitting = false;

    public static T Instance
    {
        get
        {
            if (isQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (lockObject)
            {
                if (instance == null)
                {
                    CreateInstance();
                }
                return instance;
            }
        }
    }

    public static void CreateInstance()
    {
        instance = FindAnyObjectByType<T>();

        if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
        {
            Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
            return;
        }

        if (instance == null)
        {
            GameObject singleton = new GameObject();
            instance = singleton.AddComponent<T>();
            singleton.name = "(Singleton) " + typeof(T).ToString();

            DontDestroyOnLoad(singleton);

            Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log($"[Singleton] Using instance already created: {instance.gameObject.name}");
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogWarning($"[Singleton] Instance of {typeof(T)} already exists. Destroying the new one attached to {gameObject.name}.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }
}

public class Singleton<T> where T : class, new()
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    CreateInstance();
                }
                return instance;
            }
        }
    }

    public static void CreateInstance()
    {
        instance = System.Activator.CreateInstance<T>();
    }

    protected Singleton() { }
}