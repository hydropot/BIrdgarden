using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadManager : MonoBehaviour
{

    [SerializeField] private BirdLibrary myBirdLibrary;
    [SerializeField] private StoreLibrary storeLibrary;
    [SerializeField] private PlacedLibrary placedLibrary;

    //public GameObject loadingPanel;

    public static LoadManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadGame()
    {
        if (!ES3.KeyExists("SaveData", "saveFile.json")) return;

        SaveData data = ES3.Load<SaveData>("SaveData", "saveFile.json");

        var allBirds = Resources.LoadAll<Bird>("Objects/Birds");
        myBirdLibrary.birdList = new List<Bird>();
        foreach (var birdData in data.myBirds)
        {
            var bird = allBirds.FirstOrDefault(b => b.name == birdData.name);
            if (bird != null)
            {
                bird.isLocked = birdData.isLocked;
                //bird.isPlaced = birdData.isPlaced;
                myBirdLibrary.birdList.Add(bird);
            }
        }

        var allGoods = Resources.LoadAll<Goods>("Objects/Goods");
        storeLibrary.goodsList = new List<Goods>();
        foreach (var goodsData in data.storeGoods)
        {
            var goods = allGoods.FirstOrDefault(g => g.name == goodsData.name);
            if (goods != null)
            {
                goods.isSold = goodsData.isSold;
                storeLibrary.goodsList.Add(goods);
            }
        }

        var allItems = Resources.LoadAll<ShopItem>("Objects/Shopitem");
        placedLibrary.itemList = new List<ShopItem>();
        foreach (var itemData in data.placedItems)
        {
            var item = allItems.FirstOrDefault(i => i.name == itemData.name);
            if (item != null)
            {
                item.isPlaced = itemData.isPlaced;
                item.cellPos = itemData.cellPos;
                placedLibrary.itemList.Add(item);
            }
        }

        // 先复制已有的默认字典
        var loadedCurrency = new Dictionary<CurrencyType, int>(CurrencySystem.CurrencyAmounts);
        foreach (var kvp in data.currencyAmounts)// 然后用存档数据覆盖已有的值（或追加）
        {
            loadedCurrency[kvp.Key] = kvp.Value;
        }
        CurrencySystem.CurrencyAmounts = loadedCurrency;// 最后赋值回去
        //CurrencySystem.CurrencyAmounts = new Dictionary<CurrencyType, int>(data.currencyAmounts);

        LevelSystem.XPNow = data.xpNow;
        LevelSystem.Level = data.level;
        PlayerPrefs.SetString("LastCloseTime", data.lastCloseTime);
        PlayerPrefs.Save();
        Debug.Log("加载了" + "等级"+ data.level+","+data.xpNow + "钱" + data.currencyAmounts.Count + "时间" + data.lastCloseTime);

        CurrencySystem.Instance.Refresh();
        LevelSystem.Instance.UpdateUI();
        Initializer.current.Init();
        BirdSpawner.current.InitBird();

        UISystem.current.loadingPanel.SetActive(false);

        /*
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            // 第一次登录，设置初始资金
            if (!CurrencySystem.CurrencyAmounts.ContainsKey(CurrencyType.Coins))
                CurrencySystem.CurrencyAmounts[CurrencyType.Coins] = 200;
            else
                CurrencySystem.CurrencyAmounts[CurrencyType.Coins] += 200;

            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            PlayerPrefs.Save();

            Debug.Log("首次登录，赠送 200 Coins");
        }
        */

    }
}
