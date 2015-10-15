using UnityEngine;
using System.Collections;

public class DrawSpots : MonoBehaviour {

    public GameObject RedSpot;
    public GameObject YellowSpot;
    public GameObject BlueSpot;
    public GameObject GreenSpot;
    public GameObject PurpleSpot;
    public GameObject OrangeSpot;
    public GameObject ClearSpot;

    public GameObject ReturnSpot(string ColorString) {
        switch(ColorString) {
            case "Red":
                return RedSpot;
            case "Yellow":
                return YellowSpot;
            case "Blue":
                return BlueSpot;
            case "Green":
                return GreenSpot;
            case "Orange":
                return OrangeSpot;
            case "Purple":
                return PurpleSpot;
            case "":
                return ClearSpot;
        }
        return gameObject;
    }
}
