﻿using UnityEngine;
using System.Collections;

public class BillboardScript : MonoBehaviour {

    public Camera m_Camera;

    void Start() {
        m_Camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
            m_Camera.transform.rotation * Vector3.up);
    }
}
