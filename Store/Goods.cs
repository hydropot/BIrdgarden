using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Goods",menuName = "Store/New Goods") ]
public class Goods : ScriptableObject
{
    public string goodsName;
    
    public Sprite goodsSprite;
    
    public int goodsPrice;
    
    [TextArea]
    public string goodsInfo;

    public ShopItem linkedShopItem;

    public bool isSold;

}
