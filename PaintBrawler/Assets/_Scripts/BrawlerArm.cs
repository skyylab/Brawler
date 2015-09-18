using UnityEngine;
using System.Collections;

public class BrawlerArm : MonoBehaviour {
    [SerializeField]
    private GameObject _Parent;
    private string Color;

    void Start() {
        ChangeColor(_Parent.GetComponent<HeroScript>().SecondaryIterator);
    } 

    public void ChangeColor (int ColorNumber) {
        switch (ColorNumber)
        {
            case 0:
                Color = "Orange";
                break;

            case 1:
                Color = "Green";
                break;

            case 2:
                Color = "Purple";
                break;
        }
    }

	void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy") {
            other.transform.gameObject.GetComponent<Rigidbody>().AddForce((_Parent.transform.forward + Vector3.up) * 100);
        }

    }

}
