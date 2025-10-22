using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealthUI : MonoBehaviour
{
    public ShowText Text;
    public int playerVida;
    public bool gameOver;
    public LifeUI firstNum;
    public LifeUI secondNum;
    public LifeUI thirdNum;

    public float intervalDany = 1f;

    void Start()
    {
        playerVida = 100;
        gameOver = false;
        intervalDany = 1f; // força el valor
        UpdateLifeUI();
        StartCoroutine(DecrementarVidaCadaSegon());
    }

    private IEnumerator DecrementarVidaCadaSegon()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(intervalDany);

            playerVida -= 1;
            playerVida = Mathf.Max(playerVida, 0);
            UpdateLifeUI();

            if (playerVida <= 0)
                fiPartida();
        }
    }

    public void UpdateLifeUI()
    {
        firstNum.changeLife((playerVida / 100) % 10);
        secondNum.changeLife((playerVida / 10) % 10);
        thirdNum.changeLife(playerVida % 10);
    }

    void fiPartida()
    {
        if (!gameOver)
        {
            gameOver = true;
            Debug.Log("Fi de la partida");
            Text.endGame();
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Death") && !gameOver)
        {
            playerVida = 0;
            UpdateLifeUI();
            fiPartida();
            Debug.Log("Fi de la partida per col·lisió amb Death");
        }
    }

    int getLife()
    {
        return playerVida;
    }
}
