using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject canvas;

    private void Awake()
    {
        //initialize fields
        current = this;
        //initialize
        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
    }

    public void GetXP(int amount)
    {
        XPAddedGameEvent info = new XPAddedGameEvent(amount);
        EventManager.Instance.QueueEvent(info);
    }

    public void GetCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Coins);
        
        EventManager.Instance.QueueEvent(info);
    }

    public void GetCrystals(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Crystals);

        EventManager.Instance.QueueEvent(info);
    }
}
