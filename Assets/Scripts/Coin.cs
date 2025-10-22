using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public bool coinGotten = false;
    public float duracioAnimacio = 3f;
    public float velocitatRotacio = 360; // graus per segon
    public float velocitatAscens = 100f;    // unitats per segon

    private bool animant = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coinGotten || animant) return;

        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                coinGotten = true;
                player.AddCoin(); // suma una moneda al jugador
            }

            StartCoroutine(AnimacioRecollida());
        }
    }

    private IEnumerator AnimacioRecollida()
    {
        animant = true;

        float temps = 0f;
        while (temps < duracioAnimacio)
        {
            transform.Rotate(Vector3.up * 30f * velocitatRotacio * Time.deltaTime);
            transform.position += Vector3.up * 5f* velocitatAscens * Time.deltaTime;
            temps += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // elimina la moneda després de l'animació
    }
}
