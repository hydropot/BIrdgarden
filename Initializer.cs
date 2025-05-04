using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private StoreLibrary myLibrary;
    [SerializeField] private StoreLibrary storeLibrary;
    [SerializeField] private PlacedLibrary myplacedLibrary;
    [SerializeField] private PlacedLibrary placedLibrary;
    [SerializeField] private BirdLibrary mybirdLibrary;
    [SerializeField] private BirdLibrary birdLibrary;

    //public int maxcount;
    public static Initializer current;
    private void Awake()
    {
        //initialize fields
        current = this;
    }
    public void Init()
    {
        InitLibrary();
        //InitBirds(maxcount);
        AudioManager.instance.PlayBGM("BGM");
        Debug.Log("��ʼ�����");
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
    }

}
