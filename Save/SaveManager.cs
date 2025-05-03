using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private BirdLibrary myBirdLibrary;
    [SerializeField] private StoreLibrary storeLibrary;
    [SerializeField] private PlacedLibrary placedLibrary;

    public static SaveManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.myBirds = myBirdLibrary.birdList.Select(bird => new BirdSaveData
        {
            name = bird.name,
            isLocked = bird.isLocked,
            //isPlaced = bird.isPlaced
        }).ToList();

        data.storeGoods = storeLibrary.goodsList.Select(goods => new GoodsSaveData
        {
            name = goods.name,
            isSold = goods.isSold
        }).ToList();

        data.placedItems = placedLibrary.itemList.Select(item => new ShopItemSaveData
        {
            name = item.name,
            isPlaced = item.isPlaced,
            cellPos = item.cellPos
        }).ToList();

        data.currencyAmounts = CurrencySystem.CurrencyAmounts;
        data.xpNow = LevelSystem.XPNow;
        data.level = LevelSystem.Level;
        data.lastCloseTime = DateTime.Now.ToString("O");
        Debug.Log("存储了" + "等级" + data.level + "," + data.xpNow + "钱" + data.currencyAmounts + "时间" + data.lastCloseTime);
        ES3.Save("SaveData", data, "saveFile.json");
        Debug.Log(ES3Settings.defaultSettings.path);
    }
}
