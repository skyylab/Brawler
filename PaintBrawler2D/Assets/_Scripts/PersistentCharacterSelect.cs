using UnityEngine;
using System.Collections;

public class PersistentCharacterSelect : MonoBehaviour {

    [SerializeField]
    private int _brawler = -1;
    [SerializeField]
    private int _sharpShooter = -1;
    [SerializeField]
    private int _mage = -1;

    [SerializeField]
    private GameObject _selectP1;
    [SerializeField]
    private GameObject _selectP2;
    [SerializeField]
    private GameObject _selectP3;

    [SerializeField]
    private float _waitBeforeLoad = 1f;

    [SerializeField]
    private bool _loadCharacters = false;

    private GameObject _audio;

    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        _audio = GameObject.Find("MenuAudio");
    }

    public void SetPlayer(int SetInt, int PlayerNumber) {

        switch (SetInt) {
            case 0:
                _brawler = PlayerNumber;
                break;
            case 1:
                _sharpShooter = PlayerNumber;
                break;
            case 2:
                _mage = PlayerNumber;
                break;
        }
    }

    public int GetBrawler() {
        return _brawler;
    }
    public int GetShooter()
    {
        return _sharpShooter;
    }
    public int GetMage()
    {
        return _mage;
    }

	// Update is called once per frame
	void Update () {
        if (_brawler > -1 && _sharpShooter > -1 && _mage > -1 && !_loadCharacters) {
            _waitBeforeLoad -= Time.deltaTime;

            _audio.GetComponent<AudioSource>().volume -= (_audio.GetComponent<AudioSource>().volume/100) / _waitBeforeLoad;

            if (_waitBeforeLoad < 0) {
                _loadCharacters = true;
                Application.LoadLevel(Application.loadedLevel + 1);
                _loadCharacters = true;
            }
        }
        else {
            _waitBeforeLoad = 2;
        }
	}
}
