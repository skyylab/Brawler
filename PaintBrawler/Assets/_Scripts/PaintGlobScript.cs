using UnityEngine;
using System.Collections;

public class PaintGlobScript : MonoBehaviour {

    public string Color;
    public string[] ColorFields = { "Yellow", "Blue", "Red" };
    [SerializeField]
    private Color[] PrimaryColors;
    [SerializeField]
    private Color[] SecondaryColors;

    private float _DeathTimer = 2f;

    // Use this for initialization
    void Start () {
	
	}

    public void InitializeColor(int ColorIterator) {
        GetComponent<Renderer>().material.color = PrimaryColors[ColorIterator];
        Color = ColorFields[ColorIterator];
    }
	
	// Update is called once per frame
	void Update () {
        transform.position -= transform.forward;

        _DeathTimer -= Time.deltaTime;

        if (_DeathTimer < 0) {
            Destroy(gameObject);
        }                           
	}
}
