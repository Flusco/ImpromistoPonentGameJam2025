using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Vida i explosió")]
    public float lifetime = 5f; // Pots ajustar des de l’inspector
    public ParticleSystem explosionEffect; // Assigna el prefab del particle system

    private bool hasExploded = false;
    private bool canExplode = false;

    void Start()
    {
        // Destrueix automàticament si no toca res
        Destroy(gameObject, lifetime);
        StartCoroutine(EnableExplosion());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canExplode) return;
        if (collision.gameObject.CompareTag("Player")) return;
        Explode();
    }

    IEnumerator EnableExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        canExplode = true;
    }

    void OnBecameInvisible()
    {
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return; // Evita múltiples explosions
        hasExploded = true;

        // Desactivar la visual de la bala i el collider immediatament
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Instancia l'efecte d'explosió
        if (explosionEffect != null)
        {
            ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        // Destrueix la bala després d’un curt delay per assegurar que l’efecte funcioni
        Destroy(gameObject, 0.1f);
    }
}
