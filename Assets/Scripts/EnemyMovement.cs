using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public Transform leftLimit;
    public Transform rightLimit;
    public GameObject explosionEffect; // Prefab d’explosió

    private bool movingRight = true;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 moveDir = movingRight ? Vector3.right : Vector3.left;
        transform.position += moveDir * speed * Time.deltaTime;

        float scaleX = movingRight ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x);
        transform.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);

        // Canvi de direcció si està a menys de 3f del límit
        if (movingRight && Vector3.Distance(transform.position, rightLimit.position) <= 3f)
            movingRight = false;
        else if (!movingRight && Vector3.Distance(transform.position, leftLimit.position) <= 3f)
            movingRight = true;

        // Detecció d’enemic davant
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            movingRight = !movingRight;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Instancia l’explosió
            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);

            Destroy(collision.gameObject); // Elimina la bala
            StartCoroutine(RotarIExplosio());
        }
    }

    private IEnumerator RotarIExplosio()
    {
        float duracioRotacio = 0.5f;
        float velocitatRotacio = 720f; // graus per segon

        float temps = 0f;
        while (temps < duracioRotacio)
        {
            transform.Rotate(Vector3.up * velocitatRotacio * Time.deltaTime);
            temps += Time.deltaTime;
            yield return null;
        }

        // Instancia l’explosió
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject); // Elimina l’enemic
    }

}
