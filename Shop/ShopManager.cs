using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private StoreLibrary storeLibrary;
    //singletone pattern
    public static ShopManager current;

    //currency sprites for initialization
    public static Dictionary<CurrencyType, Sprite> currencySprites = new Dictionary<CurrencyType, Sprite>();
    [SerializeField] private List<Sprite> sprites;

    //fields for animating the shop window
    private RectTransform rt;
    private RectTransform prt;
    private bool opened;

    //prefab for displaying shop info
    [SerializeField] private GameObject itemPrefab;
    //al shop items
    private Dictionary<ObjectType, List<ShopItem>> shopItems = new Dictionary<ObjectType, List<ShopItem>>(5);

    //tabs with items
    [SerializeField] public TabGroup shopTabs;

    private void Awake()
    {
        //initialize fields
        current = this;

        rt = GetComponent<RectTransform>();
        prt = transform.parent.GetComponent<RectTransform>();

        //subscribe method to an event
        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
    }

    private void Start()
    {
        //add sprites
        currencySprites.Add(CurrencyType.Coins, sprites[0]);
        currencySprites.Add(CurrencyType.Crystals, sprites[1]);

        //load shop items and initialize the shop
        Load();
        Initialize();

        //disable the shop window so the tabs are not visible
        gameObject.SetActive(false);
    }

    private void Load()
    {
        // 初始化字典（清空并重建）
        shopItems.Clear();
        shopItems.Add(ObjectType.Bowls, new List<ShopItem>());
        shopItems.Add(ObjectType.Platforms, new List<ShopItem>());
        shopItems.Add(ObjectType.Trees, new List<ShopItem>());
        shopItems.Add(ObjectType.Toys, new List<ShopItem>());
        shopItems.Add(ObjectType.Decorations, new List<ShopItem>());

        // 遍历 StoreLibrary 中的所有 Goods
        foreach (Goods goods in storeLibrary.goodsList)
        {
            if (goods != null && goods.linkedShopItem != null)
            {
                ObjectType type = goods.linkedShopItem.Type;

                if (shopItems.ContainsKey(type))
                {
                    shopItems[type].Add(goods.linkedShopItem);
                }
                else
                {
                    Debug.LogWarning($"未知类型的 ShopItem：{goods.linkedShopItem.name}，类型是 {type}");
                }
            }
            else
            {
                Debug.LogWarning($"Goods 或 linkedShopItem 为 null：{goods?.name}");
            }
        }
    }

    public void ReloadShop()
    {
        // 清除已有 UI 元素
        foreach (GameObject tabObject in shopTabs.objectsToSwap)
        {
            foreach (Transform child in tabObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // 重新加载并初始化
        Load();
        Initialize();
    }



    private void Initialize()
    {
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            foreach (var item in shopItems[(ObjectType)i])
            {
                //create an item holder and initialize it
                GameObject itemObject = Instantiate(itemPrefab, shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ShopItemHolder>().Initialize(item);
            }
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        //when the player gets a new level
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            ObjectType key = shopItems.Keys.ToArray()[i];
            for (int j = 0; j < shopItems[key].Count; j++)
            {
                ShopItem item = shopItems[key][j];

                if (item.Level == info.newLvl)
                {
                    //unlock item if its level matches the new one
                    shopTabs.transform.GetChild(i).GetChild(j).GetComponent<ShopItemHolder>().UnlockItem();
                }
            }
        }
    }

    public void ShopButton_Click()
    {
        //animation time
        float time = 0.2f;
        if (!opened)
        {
            //open the shop
            LeanTween.moveY(prt, prt.anchoredPosition.y + rt.sizeDelta.y, time);
            opened = true;
            gameObject.SetActive(true);

            RefreshAllItemPlacementUI();
            //Debug.Log("打开了");
        }
        else
        {
            //close the shop
            LeanTween.moveY(prt, prt.anchoredPosition.y - rt.sizeDelta.y, time)
                .setOnComplete(delegate ()
                {
                    gameObject.SetActive(false);
                });
            opened = false;
            //Debug.Log("关上了");
        }
    }

    //make the shop close when the player click on the area
    private bool dragging;

    public void OnBeginDrag()
    {
        dragging = true;
    }

    public void OnEndDrag()
    {
        dragging = false;
    }

    public void OnPointerClick()
    {
        if (!dragging)
        {
            ShopButton_Click();
            Debug.Log("OnPointerClick");
        }
    }

    private void RefreshAllItemPlacementUI()
    {
        for (int i = 0; i < shopTabs.objectsToSwap.Count; i++)
        {
            foreach (Transform child in shopTabs.objectsToSwap[i].transform)
            {
                ShopItemHolder holder = child.GetComponent<ShopItemHolder>();
                if (holder != null)
                {
                    holder.RefreshPlacementUI();
                }
            }
        }
    }

}
