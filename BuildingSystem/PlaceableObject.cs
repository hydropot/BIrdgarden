﻿using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    private Vector3 origin;
    public BoundsInt area;
    public ShopItem item;
    public PlacedLibrary myplacedlibrary;

    private float pressTime = 0f;
    private bool isPressing = false;

    public static bool buildingmode = false;
    public bool CanBePlaced()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return BuildingSystem.current.CanTakeArea(areaTemp);
    }

    public void Place()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Debug.Log($"放置在了网格坐标: {positionInt}");
        AudioManager.instance.PlayFX("drop5");
        item.cellPos = positionInt;

        Placed = true;
        origin = transform.position;

        BuildingSystem.current.TakeArea(areaTemp);
        item.isPlaced = true;
        // 只有当 item 不在 myplacedlibrary.itemList 中时才执行添加
        if (!myplacedlibrary.itemList.Contains(item))
        {
            myplacedlibrary.itemList.Add(item);
            
        }

    }


    public void CheckPlacement()
    {
        if (!Placed)
        {
            if (CanBePlaced())
            {
                Place();
            }
            else
            {
                Destroy(gameObject);
            }

            //ShopManager.current.ShopButton_Click();
        }
        else
        {
            if (!CanBePlaced())
            {
                transform.position = origin;
            }

            Place();
        }
    }

    private void Update()
    {
        if (BuildingSystem.BuildingRoot == null || !BuildingSystem.BuildingRoot.activeSelf)
        {
            buildingmode = false;
        }
        else
        {
            buildingmode = true;
        }
        DetectLongPress();
    }

    private void DetectLongPress()
    {
        if (!Placed) return;

        // 使用集中管理的 BuildingRoot
        //if (BuildingSystem.isBuildMode == null || !BuildingSystem.isBuildMode.activeSelf) return;
        if (!buildingmode) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                isPressing = true;
                pressTime = 0f;
            }
        }

        if (Input.GetMouseButton(0) && isPressing)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= 2f)
            {
                isPressing = false;

                Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
                BoundsInt areaTemp = area;
                areaTemp.position = positionInt;

                BuildingSystem.current.ReleaseArea(areaTemp);

                Placed = false;
                gameObject.AddComponent<ObjectDrag>();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPressing = false;
        }
    }
}
