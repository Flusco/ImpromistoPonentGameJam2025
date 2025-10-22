using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Image lifeNum;
    public Sprite[] spriteVida;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeLife(int life)
    {
        lifeNum.sprite = spriteVida[life];
    }
}
