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
    private bool _pressSpecial;

    private HeroScript _heroScript;

    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private Vector3 _mainCameraPosition;

    private float _restrictXLeft;
    private float _restrictXRight;

    private float _chargeThresholdTimer = 0.2f;
    private float _chargeThresholdTimerReset;

    void Awake()
    {
        // Get the character controller
        _heroScript = GetComponent<HeroScript>();
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _chargeThresholdTimerReset = _chargeThresholdTimer;
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

        _mainCameraPosition = _mainCamera.transform.position;

        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        _pressAttack1 = _player.GetButton(Actions.Default.PrimaryAttack);
        _pressAttack2 = _player.GetButton(Actions.Default.SecondaryAttack);
        _pressJump = _player.GetButtonDown(Actions.Default.Jump);
        _pressSpecial = _player.GetButtonDown(Actions.Default.SpecialAttack);

        _moveVector.x = _player.GetAxis(Actions.Default.MoveHoriz);
        _moveVector.y = _player.GetAxis(Actions.Default.MoveVert);
    }

    private void ProcessInput()
    {
        if (_heroScript.ReturnIsAlive())
        {
            if (_pressAttack1)
            {
                _chargeThresholdTimer -= Time.deltaTime;

                if (_chargeThresholdTimer < 0) {
                    _heroScript.ChargeAttack();
                }
                else
                {
                    _heroScript.Attack();
                    _heroScript.ChargeAttackReset();
                }
            }
            else
            {
                _heroScript.ResetChargeAttack();
                _chargeThresholdTimer = _chargeThresholdTimerReset;
            }

            if (_pressAttack2)
            {
                _heroScript.SecondaryAttack();
            }

            if (_pressJump)
            {
                _heroScript.Jump();
            }

            if (_pressSpecial && !_heroScript.GetSpecialStatus() && _heroScript.CanActivateSpecial())
            {
                _heroScript.SpecialAttack();
            }


            // Process movement
            if (_moveVector.x != 0.0f || _moveVector.y != 0.0f)
            {
                ProcessMovement();
                _heroScript.PlayWalkAnim();
            }
            else
            {

                _heroScript.PlayIdleAnim();
            }

        }
    }

    private void ProcessMovement() {
        if (transform.position.z > -12 && _moveVector.y < 0)
        {
            transform.position += new Vector3(0f, 0f, _moveVector.y) * moveSpeed * (0.65f);
        }
        if (transform.position.z < 2 && _moveVector.y > 0)
        {
            transform.position += new Vector3(0f, 0f, _moveVector.y) * moveSpeed * (0.65f);
        }

        if (_mainCameraPosition.x - transform.position.x > -11 && _moveVector.x > 0)
        {
            transform.position += new Vector3(_moveVector.x, 0f, 0f) * moveSpeed;
        }

        if (_mainCameraPosition.x - transform.position.x < 11 && _moveVector.x < 0)
        {
            transform.position += new Vector3(_moveVector.x, 0f, 0f) * moveSpeed;
        }
        _heroScript.ManageFlipSprite(_moveVector);
    }
}
