using UnityEngine;
using System.Collections;

public class SectionManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] _attackerWaves;
    private GameObject _mainCamera;
    [SerializeField]
    private int _iterator = 0;

    void Start()
    {
        _attackerWaves[_iterator].SetActive(true);
        _mainCamera = Camera.main.gameObject;
    }

	// Update is called once per frame
	void Update () {
	    if (_attackerWaves[_iterator].transform.childCount <= 0)
        {
            _iterator++;
            if (_iterator < _attackerWaves.Length)
            {
                _attackerWaves[_iterator].SetActive(true);
            }
            else
            {
                _mainCamera.GetComponent<CameraControls>().SetCameraLock(false);
                gameObject.SetActive(false);
            }
        }
	}
}
