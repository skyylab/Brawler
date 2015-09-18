using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {


    [SerializeField]
    private string _ClassType;
    [SerializeField]
    private GameObject _ObjectModel;
    [SerializeField]
    private Animator _ObjectAnimator;


    [SerializeField]
    private int _ComboCounter = 0;
    [SerializeField]
    private float _ComboTimer = 2f;
    [SerializeField]
    private float _ComboTimerReset = 2f;

    [SerializeField]
    private GameObject _PaintProjectile;

    [SerializeField]
    private Color[] PrimaryColors;
    [SerializeField]
    public int PrimaryIterator = 1;
    [SerializeField]
    private Color[] SecondaryColors;
    [SerializeField]
    public int SecondaryIterator = 1;

    [SerializeField]
    private GameObject LeftArm;
    [SerializeField]
    private GameObject RightArm;
    [SerializeField]
    private GameObject Body;
    [SerializeField]
    private GameObject Head;

    // Use this for initialization
    void Start () {
        _ObjectAnimator = _ObjectModel.GetComponent<Animator>();
        _ObjectAnimator.CrossFade("Walk", 0.2f);
    }
	
	// Update is called once per frame
	void Update () {
        ManageCombo();
	}

    void ManageCombo() {

        if (_ComboTimer > 0) {
            _ComboTimer -= Time.deltaTime;
        }
        else {
            _ComboCounter = 0;

            LeftArm.GetComponent<BoxCollider>().enabled = false;
            RightArm.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void InitializeClass(string ClassType){
        switch (ClassType){
            case "Brawler":
                break;
            case "Gunner":
                break;
            case "Mage":
                break;
            case "Support":
                break;
        }
    }

    public void BrawlerAttack()
    {
        _ComboTimer = _ComboTimerReset;
        switch (_ComboCounter) {
            case 0:
                _ObjectAnimator.CrossFade("Attack 1", 0.01f);
                LeftArm.GetComponent<BoxCollider>().enabled = true;
                break;
            case 1:
                _ObjectAnimator.CrossFade("Attack 2", 0.01f);
                RightArm.GetComponent<BoxCollider>().enabled = true;
                break;
            case 2:
                _ObjectAnimator.CrossFade("Attack 3", 0.01f);
                LeftArm.GetComponent<BoxCollider>().enabled = true;
                RightArm.GetComponent<BoxCollider>().enabled = true;
                _ComboTimer = 0.5f;
                break;
            default:
                _ComboCounter = 0;
                _ComboTimer = 0f;
                break;
        }
        
        _ComboCounter++;
    }

    public void FirePaint() {
        GameObject PaintGlob = Instantiate(_PaintProjectile, transform.position, transform.rotation) as GameObject;
        PaintGlob.GetComponent<PaintGlobScript>().InitializeColor(PrimaryIterator);
    }

    public void SwitchPrimaryAttackPaint(){
        
        LeftArm.GetComponent<Renderer>().materials[1].color = SecondaryColors[SecondaryIterator];
        RightArm.GetComponent<Renderer>().materials[1].color = SecondaryColors[SecondaryIterator];
        Head.GetComponent<Renderer>().materials[2].color = SecondaryColors[SecondaryIterator];
        Body.GetComponent<Renderer>().materials[0].color = SecondaryColors[SecondaryIterator];

        LeftArm.GetComponent<BrawlerArm>().ChangeColor(SecondaryIterator);
        RightArm.GetComponent<BrawlerArm>().ChangeColor(SecondaryIterator);

        SecondaryIterator++;
        if (SecondaryIterator > 2) {
            SecondaryIterator = 0;
        }
    }

    public void SwitchSecondaryAttackPaint() {

        Head.GetComponent<Renderer>().materials[0].color = PrimaryColors[PrimaryIterator];
        Body.GetComponent<Renderer>().materials[2].color = PrimaryColors[PrimaryIterator];

        PrimaryIterator++;
        if (PrimaryIterator > 2)
        {
            PrimaryIterator = 0;
        }
    }

}
