namespace Giverspace {
    using UnityEngine;

    public class LoggerLifetime : MonoBehaviour {
        void Awake () {
            DontDestroyOnLoad(gameObject);
        }
        void OnApplicationQuit () {
            Log.Exit();
        }
    }
}