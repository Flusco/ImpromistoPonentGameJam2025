using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public Text textElement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void endGame()
    {
        textElement.text = "HAS PERDUT!";
    }
}
