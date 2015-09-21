using UnityEngine;

public class LaunchRewiredOnAwake : MonoBehaviour {
    [SerializeField] GameObject _rewiredPrefab;
    public Rewired.InputManager InputManager {
        get { return _rewiredPrefab.GetComponent<Rewired.InputManager>(); }
    }
    static GameObject _rewiredInstance;

    void Awake () {
        if (_rewiredInstance == null) {
            _rewiredInstance = Instantiate(_rewiredPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        } else {
            Destroy(gameObject);
        }
    }
}