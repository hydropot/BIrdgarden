using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoreLibrary", menuName = "StoreLibrary/New StoreLibrary")]
public class StoreLibrary : ScriptableObject
{
    public List<Goods> goodsList = new List<Goods>();
}
