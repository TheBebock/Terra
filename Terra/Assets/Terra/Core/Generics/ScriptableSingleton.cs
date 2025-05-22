using UnityEngine;

namespace Terra.Core.Generics
{
    public class ScriptableSingleton<T> : ScriptableObject where T : Object
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                string inferredPath = typeof(T).Name;
                GetInstance(inferredPath);

                return _instance;
            }
        }
        
        private static T GetInstance(string path)
        {
            if (_instance == null)
            {
                _instance = Resources.Load<T>(path);
                if (_instance == null)
                    Debug.LogError("Called singleton doesn't exist!: " + typeof(T));
            }

            return _instance;
        }
    }
}