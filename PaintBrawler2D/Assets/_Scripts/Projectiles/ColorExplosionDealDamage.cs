using UnityEngine;
using System.Collections;

public class ColorExplosionDealDamage : MonoBehaviour {

    private int _damage;
    private Color _explosionColor;

    public void Initialize(int Damage, Color ExplosionColor)
    {
        _damage = Damage;
        _explosionColor = ExplosionColor;
        GetComponent<ParticleSystem>().startColor = ExplosionColor;
    }

    void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<BasicEnemySingleColor>().TakeDamage(_damage, _explosionColor);
        }
    }
}
