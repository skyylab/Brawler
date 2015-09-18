using UnityEngine;
using System.Collections;
using Rewired;
using RewiredConstants;

public class PlayerMovement : MonoBehaviour {

    public int playerId = 0; // The Rewired player id of this character

    public float moveSpeed = 3.0f;
    public float bulletSpeed = 15.0f;
    public GameObject bulletPrefab;

    private Player player; // The Rewired Player
    private Vector3 moveVector;
    private bool fire;
    private bool fire2;

    private bool switchRB;
    private bool switchLB;

    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    void Awake()
    {
        // Get the character controller
    }

    private void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);

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

        //moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
       // moveVector.y = player.GetAxis("Move Vertical");
        fire = player.GetButtonDown(Actions.Default.Fire);
        fire2 = player.GetButtonDown(Actions.Default.FirePaint);

        switchRB = player.GetButtonDown(Actions.Default.SwitchPaintPrimary);
        switchLB = player.GetButtonDown(Actions.Default.SwitchPaintSecondary);

        moveVector.x = player.GetAxis(Actions.Default.MoveX);
        moveVector.y = player.GetAxis(Actions.Default.MoveY);
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            transform.position += new Vector3(moveVector.x, 0f, moveVector.y) * moveSpeed;
            transform.forward = Vector3.Normalize(new Vector3(-moveVector.x * 100, 0f, -moveVector.y * 100));
        }

        // Process fire
        if (fire)
        {
            GetComponent<HeroScript>().BrawlerAttack();
        }

        if (fire2) {
            GetComponent<HeroScript>().FirePaint();
        }

        if (switchRB) {
            GetComponent<HeroScript>().SwitchPrimaryAttackPaint();
        }

        if (switchLB){
            GetComponent<HeroScript>().SwitchSecondaryAttackPaint();
        }
    }
}
