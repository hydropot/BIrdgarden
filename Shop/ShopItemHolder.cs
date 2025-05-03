using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemHolder : MonoBehaviour
{
    //save the item
    private ShopItem Item;

    //fields for the UI
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image currencyImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject UIBlock;

    public void Initialize(ShopItem item)
    {
        Item = item;

        //initialize UI
        iconImage.sprite = Item.Icon;
        titleText.text = Item.Name;
        descriptionText.text = Item.Description;
        currencyImage.sprite = ShopManager.currencySprites[Item.Currency];
        priceText.text = Item.Price.ToString();
        //UIBlock.SetActive(false);
        UnlockItem();


        /*
        if (Item.Level >= LevelSystem.Level)
        {
            UnlockItem();
        }
        */
    }

    public void UnlockItem()
    {
        //add shop drag to the icon
        //iconImage.gameObject.AddComponent<ShopItemDrag>();
        iconImage.gameObject.AddComponent<ShopItemDrag>().Initialize(Item);
        //enable the arrow on the side of the icon
        //iconImage.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void RefreshPlacementUI()
    {
        UIBlock.SetActive(Item.isPlaced);
    }
}
