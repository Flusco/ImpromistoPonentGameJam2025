using UnityEngine;
using UnityEngine.SceneManagement;

public class FInalTrigger : MonoBehaviour
{
    public PlayerMovement player;
    public ShowText text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.GetCoinCount() == 3)
        {
            SceneManager.LoadScene("photoScene");
        } else if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("photoSceneTrophy");
        }
    }
}
