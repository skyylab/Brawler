using UnityEngine;
using System.Collections;

public class StunRange : MonoBehaviour {

    [SerializeField]
    private GameObject _parent;
    private BrawlerClass _parentScript;

    [SerializeField]
    private GameObject _powPrefab;

    [SerializeField]
    private AudioClip[] _hitSFX;

    private AudioSource _audio;
    void Start()
    {
        _parentScript = _parent.GetComponent<BrawlerClass>();
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().Stun(4f);
            Instantiate(_powPrefab, transform.position + transform.right, transform.rotation);
        }
    }
}
