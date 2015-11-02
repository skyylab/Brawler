using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReviveScript : MonoBehaviour {

    [SerializeField]
    private int _reviveThreshold = 0;
    private int _reviveThresholdMax = 1000;
    [SerializeField]
    private GameObject _reviveBar;
    [SerializeField]
    private GameObject _player;

    private bool _playerReviving;

    void Start()
    {
        _reviveBar.GetComponent<Slider>().maxValue = _reviveThresholdMax;
        _reviveBar.GetComponent<Slider>().value = _reviveThreshold;
    }

    void Update()
    {
        if (!_playerReviving)
        {
            _reviveThreshold--;
            _reviveBar.GetComponent<Slider>().value = _reviveThreshold;
        }
    }

	void OnTriggerStay(Collider other)
    {
        _playerReviving = true;
        if (other.tag == "Player")
        {
            _reviveThreshold += 2;
            _reviveBar.GetComponent<Slider>().value = _reviveThreshold;
        }

        if (_reviveThreshold > _reviveThresholdMax)
        {
            _reviveThreshold = 0;
            _player.GetComponent<HeroScript>().RevivePlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        _playerReviving = false;
    }
}
