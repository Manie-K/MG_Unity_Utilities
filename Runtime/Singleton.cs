using UnityEngine;

namespace MG_Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if(Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogError("Singleton already created");
            }
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if(Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                base.Awake();
            }
            else
            {
                Destroy(gameObject);
                Debug.LogError("Singleton already created");
            }
        }
    }
}
