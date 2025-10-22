using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playGame()
    {

        SceneManager.LoadScene("videoScene");
    }
    public void closeGame()
    {
        Application.Quit();
    }
}
