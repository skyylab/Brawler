using UnityEngine;
using System.Collections;

public class PowerUpScript : MonoBehaviour {

    [SerializeField]
    private GameObject[] _powerUpTypesSprites;

    private PowerUpType _powerUpType;

    private enum PowerUpType {
        health,
        moveSpeed,
        damage,
        defense
    }

	// Use this for initialization
	void Start () {
        RandomizeItem();
	}
	
    void RandomizeItem() {
        _powerUpType = (PowerUpType) Random.Range(0, PowerUpType.GetNames(typeof(PowerUpType)).Length);
        _powerUpTypesSprites[(int) _powerUpType].SetActive(true);
    }

	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
        Debug.Log(_powerUpType);

        if (other.tag == "Player") {
            AddPowerUp(other.gameObject);
        }
    }

    void AddPowerUp(GameObject Player) {
        switch (_powerUpType) {
            case PowerUpType.damage:
                Player.GetComponent<HeroScript>().AddDamage();
                break;
            case PowerUpType.defense:
                Player.GetComponent<HeroScript>().AddDefense();
                break;
            case PowerUpType.health:
                Player.GetComponent<HeroScript>().AddHealth();
                break;
            case PowerUpType.moveSpeed:
                Player.GetComponent<HeroScript>().AddSpeed();
                break;
        }
        gameObject.SetActive(false);
    }
}
