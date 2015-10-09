using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    [SerializeField]
    protected int _hitPoints = 100;
    private Color[] _secondaryColorArray = {new Color (57f/255f, 212f/255f, 50f/255f),
                                            new Color (130f/255f, 83f/255f, 137f/255f),
                                            new Color(234f/255f, 123f/255f, 54f/255f)};

    [SerializeField]
    protected Color _secondaryColor;
    [SerializeField]
    protected string _currentColor;

    // Use this for initialization
    void Start () {
        InitializeClass();
	}

    protected void InitializeClass()
    {
        SetInitialColors();
        // Setting range
    }

    private void SetInitialColors()
    {
        int RandomNumber = Random.Range(0, 3);
            // Setting Player Color
        _secondaryColor = _secondaryColorArray[RandomNumber];

        switch (RandomNumber)
        {
            case 0:
                _currentColor = "Green";
                break;
            case 1:
                _currentColor = "Purple";
                break;
            case 2:
                _currentColor = "Orange";
                break;
        }
        
        GetComponent<SpriteRenderer>().color = _secondaryColor;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
