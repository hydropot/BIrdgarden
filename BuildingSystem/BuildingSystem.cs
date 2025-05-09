﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    // 添加用于全局访问的 Building GameObject 引用
    public GameObject buildingRoot; //与PlaceableObject中的buildingmode有关
    public static GameObject BuildingRoot;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private BoundsInt prevArea;

    private void Awake()
    {
        current = this;
        BuildingRoot = buildingRoot; // 设置静态引用

        string tilePath = "Tiles/";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    /*
    private void Start()
    {
        string tilePath = "Tiles/";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }
    */
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileBase tileBase, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(tileArray, tileBase);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileBase tileBase)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBase;
        }
    }

    public void ClearArea(BoundsInt area, Tilemap tilemap)////设置为空 tile
    {
        SetTilesBlock(area, null, tilemap);
    }
    
    public void ReleaseArea(BoundsInt area)//释放区域时设置为白色 tile
    {
        SetTilesBlock(area, tileBases[TileType.White], MainTilemap);
    }


    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, tileBases[TileType.Green], MainTilemap);
    }

    public void InitializeWithObject(GameObject building, Vector3 pos)
    {
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        GameObject obj = Instantiate(building, position, Quaternion.identity);
        PlaceableObject temp = obj.transform.GetComponent<PlaceableObject>();
        temp.gameObject.AddComponent<ObjectDrag>();
    }

    public void FollowObject(PlaceableObject obj)
    {
        ClearArea(prevArea, TempTilemap);

        obj.area.position = gridLayout.WorldToCell(obj.transform.position);
        BoundsInt area = obj.area;
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        TileBase[] tileArray = new TileBase[baseArray.Length];
        bool canPlace = true;

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                canPlace = false;
                break;
            }
        }

        if (!canPlace)
        {
            FillTiles(tileArray, tileBases[TileType.Red]);
        }

        TempTilemap.SetTilesBlock(area, tileArray);
        prevArea = area;
    }


    //初始化时使用，不用拖动就能将物品摆在指定网格的函数
    public void PlaceObjectAtPosition(GameObject prefab, Vector3Int gridPos, PlacedLibrary placedLibrary)
    {
        Vector3 worldPos = gridLayout.CellToLocalInterpolated(gridPos + new Vector3(0.5f, 0.5f, 0));
        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity);

        PlaceableObject placeable = obj.GetComponent<PlaceableObject>();
        placeable.myplacedlibrary = placedLibrary;

        if (placeable.CanBePlaced())
        {
            placeable.Place(); // 会自动调用 TakeArea，变为绿色
            //Debug.Log("BuildingSystem放置！");
        }
        else
        {
            Debug.LogWarning("指定位置不可放置！");
            Destroy(obj);
        }
    }

}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
