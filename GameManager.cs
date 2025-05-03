using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;

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
   
    void Start()
    {
        //访客登录
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                //Debug.LogError($"LootLocker session failed. Error: {response.errorData?.message}");
                Debug.LogError($"[LootLocker] Guest Session failed. StatusCode: {response.statusCode}, Message: {response.errorData?.message}");
            
            return;
            }

            Debug.Log("successfully started LootLocker session");
        });
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


    public bool SpendCurrency(CurrencyType currencyType, int amount)
    {
        int currentAmount = CurrencySystem.CurrencyAmounts[currencyType];

        if (currentAmount >= amount)
        {
            CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-amount, currencyType);
            EventManager.Instance.QueueEvent(info);
            return true; // 扣钱成功
        }
        else
        {
            NotEnoughCurrencyGameEvent info = new NotEnoughCurrencyGameEvent(amount, currencyType);
            EventManager.Instance.QueueEvent(info);
            return false; // 扣钱失败
        }
    }


}
