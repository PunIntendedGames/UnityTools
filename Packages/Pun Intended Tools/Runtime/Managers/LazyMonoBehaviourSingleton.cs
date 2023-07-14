using UnityEngine;

namespace PunIntended.Tools
{
    public abstract class LazyMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _singleton;

        public static T Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    GameObject consoleGameObject = new(typeof(T).Name);
                    _singleton = consoleGameObject.AddComponent<T>();
                    DontDestroyOnLoad(consoleGameObject);
                }

                return _singleton;
            }
            private set
            {
                _singleton = value;
            }
        }
    }
}