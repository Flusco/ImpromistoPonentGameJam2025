using UnityEngine;
using System.Collections;

public class TrencarPedra : MonoBehaviour
{
    [Header("Temps de cicle")]
    public float tempsInvisible = 1.2f;

    private SpriteRenderer sprite;
    private Collider2D col;
    private bool cicleEnMarxa = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        sprite.enabled = true;
        col.enabled = true;
    }

    void Update()
    {
        DetectarJugadorSobre();
    }

    void DetectarJugadorSobre()
    {
        Vector3 checkPos = transform.position + new Vector3(0f, 0.6f, 0f); // zona just a sobre
        Vector2 checkSize = new Vector2(1.2f, 0.4f); // àrea de detecció
        LayerMask playerLayer = LayerMask.GetMask("Player"); // assegura’t que el jugador està en aquesta capa

        Collider2D playerCol = Physics2D.OverlapBox(checkPos, checkSize, 0f, playerLayer);

        if (playerCol != null && playerCol.CompareTag("Player") && !cicleEnMarxa)
        {
            Debug.Log("Jugador detectat sobre la pedra!");
            StartCoroutine(CiclePedra());
        }
    }

    private IEnumerator CiclePedra()
    {
        cicleEnMarxa = true;
        yield return new WaitForSeconds(0.8f);
        // Desapareix immediatament
        sprite.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(tempsInvisible);

        // Torna a aparèixer
        sprite.enabled = true;
        col.enabled = true;

        cicleEnMarxa = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, 0.6f, 0f), new Vector2(1.2f, 0.4f));
    }
}
