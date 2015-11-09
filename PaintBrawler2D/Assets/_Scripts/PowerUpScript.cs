using UnityEngine;
using System.Collections;

public class PowerUpScript : MonoBehaviour {

    [SerializeField]
    private GameObject[] _powerUpTypesSprites;
    [SerializeField]
    private GameObject _powerUpDescription;

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

        if (other.tag == "Player") {
            AddPowerUp(other.gameObject);
        }
    }

    void AddPowerUp(GameObject Player) {

        GameObject PowerUp = Instantiate(_powerUpDescription, transform.position, transform.rotation) as GameObject;
        switch (_powerUpType) {
            case PowerUpType.damage:
                Player.GetComponent<HeroScript>().AddDamage();
                PowerUp.GetComponent<NiceText>().Initialize("Red", "+" + "Damage!", 2);
                break;
            case PowerUpType.defense:
                Player.GetComponent<HeroScript>().AddDefense();
                PowerUp.GetComponent<NiceText>().Initialize("Blue", "+2 Defense!", 2);
                break;
            case PowerUpType.health:
                Player.GetComponent<HeroScript>().AddHealth();
                PowerUp.GetComponent<NiceText>().Initialize("Green", "+25 Health", 2);
                break;
            case PowerUpType.moveSpeed:
                Player.GetComponent<HeroScript>().AddSpeed();
                PowerUp.GetComponent<NiceText>().Initialize("Yellow", "+10 Speed", 2);
                break;
        }
        gameObject.SetActive(false);
    }
}
