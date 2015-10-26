using UnityEngine;
using System.Collections;

public class Finisher : MonoBehaviour {

    [SerializeField]
    protected GameObject _parent;
    protected HeroScript _parentScript;
    protected string _parentColor;

    public GameObject GetParent() { return _parent; }
    public string GetParentColor() { return _parentScript.GetPrimaryColorString(); }
}
