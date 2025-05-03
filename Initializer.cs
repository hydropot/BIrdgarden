using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Initializer : MonoBehaviour
{
    [SerializeField] private StoreLibrary myLibrary;
    [SerializeField] private StoreLibrary storeLibrary;
    [SerializeField] private PlacedLibrary myplacedLibrary;
    [SerializeField] private PlacedLibrary placedLibrary;
    [SerializeField] private BirdLibrary mybirdLibrary;
    [SerializeField] private BirdLibrary birdLibrary;

    //public int maxcount;
    
    private void OnEnable()
    {
        InitLibrary();
        //InitBirds(maxcount);
        AudioManager.instance.PlayBGM("BGM");
    }

    private void InitLibrary()
    {
        if (storeLibrary != null)
        {
            foreach (Goods goods in storeLibrary.goodsList)
            {
                if (goods != null)
                {
                    // 判断是否也存在于 myLibrary 中
                    if (myLibrary != null && myLibrary.goodsList.Contains(goods))
                    {
                        goods.isSold = true;
                    }
                    else
                    {
                        goods.isSold = false;
                    }
                }
            }
        }

        if (placedLibrary != null)
        {
            foreach (ShopItem item in placedLibrary.itemList)
            {
                if (item != null)
                {
                    // 判断是否也存在于 myLibrary 中
                    if (myplacedLibrary != null && myplacedLibrary.itemList.Contains(item))
                    {
                        item.isPlaced = true;
                        //Debug.Log($"物品{item.Name}位于: {item.cellPos}");

                        BuildingSystem.current.PlaceObjectAtPosition(item.Prefab, item.cellPos, myplacedLibrary);
                    }
                    else
                    {
                        item.isPlaced = false;
                    }
                }
            }
        }

        if (birdLibrary != null)
        {
            foreach (Bird bird in birdLibrary.birdList)
            {
                if (bird != null)
                {
                    // 判断是否也存在于 myLibrary 中
                    if (mybirdLibrary != null && mybirdLibrary.birdList.Contains(bird))
                    {
                        bird.isLocked = false;
                    }
                    else
                    {
                        bird.isLocked = true;
                    }
                }
            }
        }

        //Debug.Log("运行时初始化完成：Goods 和 ShopItem 状态已更新。");
    }

    /*
    private void InitBirds(int maxcount)
    {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("item");
        foreach (GameObject itemObj in allItems)
        {
            Transform birdseat = itemObj.transform.Find("birdseat");
            if (birdseat != null)
            {
                Debug.Log($"{itemObj.name}的birdseat在{birdseat.position}");

                int rand = Random.Range(1, maxcount + 1);// 如maxcount输入10则为1-10的随机数
                string path = $"Prefab/Bird/bird{rand}";
                GameObject birdPrefab = Resources.Load<GameObject>(path);
                if (birdPrefab != null)
                {
                    GameObject birdInstance = Instantiate(birdPrefab, birdseat.position, birdseat.rotation, birdseat);
                    birdInstance.name = birdPrefab.name; // 可选：保持命名一致
                }
                else Debug.LogWarning($"找不到预制体路径: {path}");
            }
            else Debug.LogWarning($"GameObject {itemObj.name} 没有找到 birdseat 节点");
        }

        Debug.Log("运行时初始化完成：Goods 和 ShopItem 状态已更新，Bird 初始化完毕。");
    }
    */
}
