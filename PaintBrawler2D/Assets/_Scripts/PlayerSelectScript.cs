using UnityEngine;
using System.Collections;
using Rewired;
using RewiredConstants;
using System.Collections.Generic;

public class PlayerSelectScript : MonoBehaviour {

    private Player _player; // The Rewired Player

    [SerializeField]
    private int _playerNumber;
    [SerializeField]
    private List<GameObject> _playerCharacters = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _playerCharactersAll = new List<GameObject>();
    [SerializeField]
    private GameObject _lockObj;
    [SerializeField]
    private GameObject _persistentCharSelect;

    [SerializeField]
    private GameObject[] _otherPlayerCharacters;

    [SerializeField]
    private int _numberSelected;
                            // Use this for initialization
    private bool _pressStart;
    private bool _pressA;
    private bool _pressB;

    private bool _moved = false;
    private bool _characterLock = false;
    
    private Vector3 _moveVector;

    void Start()
    {
        _player = ReInput.players.GetPlayer(_playerNumber);
        _pressStart = _player.GetButtonDown(Actions.Default.Start);
        _playerCharacters[_playerNumber].GetComponent<SpriteRenderer>().enabled = true;
        _numberSelected = _playerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessInput();
    }

    void GetInput()
    {
        _pressStart = _player.GetButtonDown(Actions.Default.Start);
        _moveVector.y = _player.GetAxis(Actions.Default.MoveVert);
        _pressA = _player.GetButtonDown(Actions.Default.Jump);
        _pressB = _player.GetButtonDown(Actions.Default.SpecialAttack);
    }

    void ProcessInput()
    {
        if (_pressStart)
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }

        if (!_characterLock) {
            if (!_moved) { 
                if (_moveVector.y > 0) {
                    _moved = true;

                    foreach (GameObject x in _playerCharacters)
                    {
                        if (x != null)
                        {
                            x.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }

                    _numberSelected++;

                    if (_numberSelected > _playerCharacters.Count - 1) {
                        _numberSelected = 0;
                    }

                    while (_playerCharacters[_numberSelected] == null) {
                        _numberSelected++;

                        if (_numberSelected > _playerCharacters.Count - 1)
                        {
                            _numberSelected = 0;
                        }
                    }

                    _playerCharacters[_numberSelected].GetComponent<SpriteRenderer>().enabled = true;
                }
                else if (_moveVector.y < 0)
                {
                    _moved = true;

                    foreach (GameObject x in _playerCharacters)
                    {
                        if (x != null) { 
                            x.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }

                    _numberSelected--;

                    if (_numberSelected < 0)
                    {
                        _numberSelected = _playerCharacters.Count - 1;
                    }

                    while (_playerCharacters[_numberSelected] == null)
                    {
                        _numberSelected--;

                        if (_numberSelected < 0)
                        {
                            _numberSelected = _playerCharacters.Count - 1;
                        }
                    }
                    _playerCharacters[_numberSelected].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        if (_pressA) {
            _characterLock = true;
            _lockObj.SetActive(true);
            _persistentCharSelect.GetComponent<PersistentCharacterSelect>().SetPlayer(_numberSelected, _playerNumber);

            foreach (GameObject x in _otherPlayerCharacters) {
                x.GetComponent<PlayerSelectScript>().DisableCharacter(_numberSelected);
            }
        }

        if (_pressB && _characterLock) {
            _characterLock = false;
            _lockObj.SetActive(false);

            _persistentCharSelect.GetComponent<PersistentCharacterSelect>().SetPlayer(_numberSelected, -1);
            foreach (GameObject x in _otherPlayerCharacters)
            {
                x.GetComponent<PlayerSelectScript>().EnableCharacter(_numberSelected);
            }
            _persistentCharSelect.GetComponent<PersistentCharacterSelect>().SetPlayer(-1, _playerNumber);
        }

        if (_moveVector.y == 0) {
            _moved = false;
        }

        Debug.Log(_numberSelected);
    }

    public void DisableCharacter(int Iterator) {

        if (Iterator == _numberSelected) {
            _playerCharacters[_numberSelected].GetComponent<SpriteRenderer>().enabled = false;

            _numberSelected++;

            if (_numberSelected > _playerCharacters.Count - 1)
            {
                _numberSelected = 0;
            }

            _playerCharacters[_numberSelected].GetComponent<SpriteRenderer>().enabled = true;
        }

        _playerCharacters[Iterator] = null;
    }

    public void EnableCharacter(int Iterator) {
        _playerCharacters[Iterator] = _playerCharactersAll[Iterator];
    }
}
