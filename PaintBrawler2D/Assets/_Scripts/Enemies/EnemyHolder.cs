using UnityEngine;
using System.Collections;

public class EnemyHolder : MonoBehaviour {

    [SerializeField]
    private GameObject Enemies;

	// Use this for initialization
	void Start () {
	
	}
	
	public void ActivateEnemies()
    {
        Enemies.SetActive(true);
    }
}
