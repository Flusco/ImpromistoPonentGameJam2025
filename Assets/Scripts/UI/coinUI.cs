using UnityEngine;
using UnityEngine.UI;
public class coinUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image coinImg;
    [SerializeField] private Sprite coinActivated;
    [SerializeField] private Coin coinCollected;

    void Update()
    {
        changeCoin();
    }

    void changeCoin()
    {
        if (coinCollected.coinGotten==true)
        {
            coinImg.sprite = coinActivated;
        } 
    }
}
