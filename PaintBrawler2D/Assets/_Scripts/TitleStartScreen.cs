using UnityEngine;
using System.Collections;
using Rewired;
using RewiredConstants;

public class TitleStartScreen : MonoBehaviour {

    private Player _player1; // The Rewired Player
                             // Use this for initialization
    private Player _player2; // The Rewired Player
                             // Use this for initialization
    private Player _player3; // The Rewired Player
                             // Use this for initialization
    private bool _pressStart1;
    private bool _pressStart2;
    private bool _pressStart3;

    void Start () {
        _player1 = ReInput.players.GetPlayer(0);
        _player2 = ReInput.players.GetPlayer(1);
        _player3 = ReInput.players.GetPlayer(2);
    }
	
	// Update is called once per frame
	void Update () {
        GetInput();
        ProcessInput();
	}

    void GetInput() {
        _pressStart1 = _player1.GetButtonDown(Actions.Default.Start);
        _pressStart2 = _player2.GetButtonDown(Actions.Default.Start);
        _pressStart3 = _player3.GetButtonDown(Actions.Default.Start);
    }

    void ProcessInput() {
        if (_pressStart1 || _pressStart2 || _pressStart3)
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }
}
