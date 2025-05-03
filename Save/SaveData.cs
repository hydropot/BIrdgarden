using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<BirdSaveData> myBirds;
    public List<GoodsSaveData> storeGoods;
    public List<ShopItemSaveData> placedItems;
    public Dictionary<CurrencyType, int> currencyAmounts;
    public int xpNow;
    public int level;
    public string lastCloseTime;
}

[Serializable]
public class BirdSaveData
{
    public string name;
    public bool isLocked;
    public bool isPlaced;
}

[Serializable]
public class GoodsSaveData
{
    public string name;
    public bool isSold;
}

[Serializable]
public class ShopItemSaveData
{
    public string name;
    public bool isPlaced;
    public Vector3Int cellPos;
}
