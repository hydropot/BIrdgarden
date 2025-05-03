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
                    // �ж��Ƿ�Ҳ������ myLibrary ��
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
                    // �ж��Ƿ�Ҳ������ myLibrary ��
                    if (myplacedLibrary != null && myplacedLibrary.itemList.Contains(item))
                    {
                        item.isPlaced = true;
                        //Debug.Log($"��Ʒ{item.Name}λ��: {item.cellPos}");

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
                    // �ж��Ƿ�Ҳ������ myLibrary ��
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

        //Debug.Log("����ʱ��ʼ����ɣ�Goods �� ShopItem ״̬�Ѹ��¡�");
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
                Debug.Log($"{itemObj.name}��birdseat��{birdseat.position}");

                int rand = Random.Range(1, maxcount + 1);// ��maxcount����10��Ϊ1-10�������
                string path = $"Prefab/Bird/bird{rand}";
                GameObject birdPrefab = Resources.Load<GameObject>(path);
                if (birdPrefab != null)
                {
                    GameObject birdInstance = Instantiate(birdPrefab, birdseat.position, birdseat.rotation, birdseat);
                    birdInstance.name = birdPrefab.name; // ��ѡ����������һ��
                }
                else Debug.LogWarning($"�Ҳ���Ԥ����·��: {path}");
            }
            else Debug.LogWarning($"GameObject {itemObj.name} û���ҵ� birdseat �ڵ�");
        }

        Debug.Log("����ʱ��ʼ����ɣ�Goods �� ShopItem ״̬�Ѹ��£�Bird ��ʼ����ϡ�");
    }
    */
}
