using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlacedLibrary", menuName = "PlacedLibrary/New PlacedLibrary")]
public class PlacedLibrary : ScriptableObject
{
    public List<ShopItem> itemList = new List<ShopItem>();
}
