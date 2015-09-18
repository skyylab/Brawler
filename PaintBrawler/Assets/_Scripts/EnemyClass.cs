using UnityEngine;
using System.Collections;

public class EnemyClass : MonoBehaviour {
    [SerializeField]
    private float _Weight;
    [SerializeField]
    private int _Health = 100;
    [SerializeField]
    private string CurrentColor = "";

    [SerializeField]
    private Color[] PrimaryColors;
    [SerializeField]
    private Color[] SecondaryColors;
	// Use this for initialization
	void Start () {
	    switch(CurrentColor) {
            case "Red":
                GetComponent<Renderer>().material.color = PrimaryColors[0];
                break;
            case "Yellow":
                GetComponent<Renderer>().material.color = PrimaryColors[1];
                break;
            case "Blue":
                GetComponent<Renderer>().material.color = PrimaryColors[2];
                break;
        }

        _Weight = GetComponent<Rigidbody>().mass;
	}
	
	void OnTriggerEnter(Collider other) {
        if (other.tag == "PaintGlob") {
            if (other.GetComponent<PaintGlobScript>().Color == "Red") {
                if (CurrentColor == "Yellow") { 
                    GetComponent<Renderer>().material.color = SecondaryColors[0];
                }

                if (CurrentColor == "Blue")
                {
                    GetComponent<Renderer>().material.color = SecondaryColors[2];
                }
            } 
            else if (other.GetComponent<PaintGlobScript>().Color == "Yellow") {
                if (CurrentColor == "Red")
                {
                    GetComponent<Renderer>().material.color = SecondaryColors[0];
                }

                if (CurrentColor == "Blue")
                {
                    GetComponent<Renderer>().material.color = SecondaryColors[1];
                }
            }
            else if (other.GetComponent<PaintGlobScript>().Color == "Blue")
            {
                if (CurrentColor == "Yellow")
                {
                    GetComponent<Renderer>().material.color = SecondaryColors[1];
                }

                if (CurrentColor == "Red")
                {
                    GetComponent<Renderer>().material.color = SecondaryColors[2];
                }
            }
        }
    }

    public void RecieveDamage(int Damage, string Color) {
        if (Color == CurrentColor) {
            Damage *= 10;
            _Weight = _Weight / 10;
        }
        _Health -= Damage;
    } 
}
