using UnityEngine;
using System.Collections;
using Rewired;
using RewiredConstants;

public class PlayerMovement : MonoBehaviour {

    public int playerId = 0; // The Rewired player id of this character

    public float moveSpeed = 3.0f;
    public float bulletSpeed = 15.0f;
    public GameObject bulletPrefab;

    private Player _player; // The Rewired Player
    private Vector3 _moveVector;

    private bool _pressJump;
    private bool _pressAttack1;
    private bool _pressAttack2;

    private HeroScript _heroScript;

    Vector3 newPosition;

    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    void Awake()
    {
        // Get the character controller
        _heroScript = GetComponent<HeroScript>();
        newPosition = transform.position;
    }

    private void Initialize()
    {
        // Get the Rewired Player object for this player.
        _player = ReInput.players.GetPlayer(playerId);

        initialized = true;
    }

    void Update()
    {
        if (!ReInput.isReady) {
            return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        }
        if (!initialized){
            Initialize(); // Reinitialize after a recompile in the editor
        }

        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        _pressAttack1 = _player.GetButtonDown(Actions.Default.PrimaryAttack);
        _pressAttack2 = _player.GetButtonDown(Actions.Default.SecondaryAttack);
        _pressJump = _player.GetButtonDown(Actions.Default.Jump);

        _moveVector.x = _player.GetAxis(Actions.Default.MoveHoriz);
        _moveVector.y = _player.GetAxis(Actions.Default.MoveVert);
    }

    private void ProcessInput()
    {
        // Process movement
        if (_moveVector.x != 0.0f || _moveVector.y != 0.0f)
        {
            if (transform.position.x < 18 && transform.position.x > -18)
            {
                transform.position += new Vector3(_moveVector.x, 0f, 0f) * moveSpeed;
            }
            else
            {
                transform.position -= new Vector3(_moveVector.x, 0f, 0f) * moveSpeed * 3;
            }

            if (transform.position.y < 4 && transform.position.y > -9)
            {
                transform.position += new Vector3(0, _moveVector.y, 0f) * moveSpeed*(0.65f);
            }
            else
            {
                transform.position -= new Vector3(0, _moveVector.y, 0f) * moveSpeed * 3;
            }

            _heroScript.ManageFlipSprite(_moveVector);
        }

        if (_pressAttack1) {
            _heroScript.Attack();
        }

        if (_pressJump) {
            _heroScript.Jump();
        } 
       
    }
}
