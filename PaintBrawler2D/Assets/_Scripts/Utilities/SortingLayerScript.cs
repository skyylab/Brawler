﻿using UnityEngine;
using System.Collections;

public class SortingLayerScript : MonoBehaviour {

    [SerializeField]
    private int SortingLayer;
    [SerializeField]
    private GameObject _parent;

	// Use this for initialization
	void Start () {
        SortingLayer = GetComponent<SpriteRenderer>().sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = SortingLayer + (int)(((100 - (_parent.transform.position.z + 9)) * 1000)/100);
	}
}
