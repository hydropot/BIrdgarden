using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GoodsSlot : MonoBehaviour
{
    public TextMeshProUGUI  nameText;
    
    public Image iconImage;

    public GameObject UIblock;

    public TextMeshProUGUI  priceText;
    
    public Goods goods;


    
    public void ItemOnClicked()
    {
        BackpackPagination.UpdateItemInfo(goods);
        AudioManager.instance.PlayFX("drop4");

    }
    public void SetupGoodsSlot(Goods targetGoods)
    {
        if (targetGoods == null)
        {
            return;
        }
        goods = targetGoods;
        
        nameText.text = goods.goodsName;

        iconImage.sprite = goods.goodsSprite;
        //iconImage.SetNativeSize();
        
        priceText.text = goods.goodsPrice.ToString();

        Button button = GetComponent<Button>();

        if (goods.isSold == true)
        {
            UIblock.SetActive(true);
            if (button != null) button.enabled = false;

        }
        else
        {
            UIblock.SetActive(false);
            if (button != null) button.enabled = true;
        }
    }
}
