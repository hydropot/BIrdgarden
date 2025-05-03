using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CurrencySystem : MonoBehaviour
{
    public static Dictionary<CurrencyType, int> CurrencyAmounts = new Dictionary<CurrencyType, int>
    {
        { CurrencyType.Coins, 0 },
        { CurrencyType.Crystals, 0 }
    };


    [SerializeField] private List<GameObject> texts;

    private Dictionary<CurrencyType, TextMeshProUGUI> currencyTexts =
        new Dictionary<CurrencyType, TextMeshProUGUI>();

    public static CurrencySystem Instance;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        currencyTexts = new Dictionary<CurrencyType, TextMeshProUGUI>
        {
            { CurrencyType.Coins, texts[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>() },
            { CurrencyType.Crystals, texts[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>() }
        };

        foreach (var kvp in currencyTexts)
        {
            var type = kvp.Key;
            var text = kvp.Value;

            if (!CurrencyAmounts.ContainsKey(type))
            {
                CurrencyAmounts[type] = 0;
            }

            text.text = CurrencyAmounts[type].ToString();
        }

        EventManager.Instance.AddListener<CurrencyChangeGameEvent>(OnCurrencyChange);
        EventManager.Instance.AddListener<NotEnoughCurrencyGameEvent>(OnNotEnough);
        
    }

    private void OnCurrencyChange(CurrencyChangeGameEvent info)
    {
        //todo save the currency
        CurrencyAmounts[info.currencyType] += info.amount;
        currencyTexts[info.currencyType].text = CurrencyAmounts[info.currencyType].ToString();

        GameManager.current?.UPSaveToCloud(); // 此时数据已更新
    }

    private void OnNotEnough(NotEnoughCurrencyGameEvent info)
    {
        Debug.Log($"You don't have enough of {info.amount} {info.currencyType}");
    }


    public void Refresh()
    {
        foreach (var kvp in currencyTexts)
        {
            var type = kvp.Key;
            kvp.Value.text = CurrencyAmounts[type].ToString();
        }
    }

}

public enum CurrencyType
{
    Coins,
    Crystals
}
