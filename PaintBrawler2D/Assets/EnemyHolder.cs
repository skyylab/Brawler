using UnityEngine;
using System.Collections;

public class EnemyHolder : MonoBehaviour {

    [SerializeField]
    private GameObject[] Enemies;

	// Use this for initialization
	void Start () {
	
	}
	
	public void ActivateEnemies()
    {
        foreach (GameObject x in Enemies)
        {
            x.SetActive(true);
        }
    }
}
