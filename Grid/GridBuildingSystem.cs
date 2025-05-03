/*
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;//单例模式

    public GridLayout gridLayout; //Grid Object
    public Tilemap MainTilemap; //Main tilemap -主要显示的网格
    public Tilemap TempTilemap; //Temp tilemap -用于提示的网格

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();//存储不同网格的静态字典

    //引用正在放置的建筑
    private Building temp; //建筑本身
    private Vector3 prevPos; //建筑的先前区域
    private BoundsInt prevArea;

    public Vector3 offset1;
    public Vector3 offset2;
    #region Unity Methods

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        //初始化网格字典
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }
    private void Update()
    {
        if (!temp)//无建筑
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            if (!temp.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {
                    //temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0f, 0f, 0f));
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + offset1);
                    prevPos = cellPos;
                    FollowBuilding();
                }


            }
        }
    }
    #endregion

    #region Tilemap Management//获取和设置瓷砖地图上的瓷砖

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        //go through each position from the area
        foreach (var v in area.allPositionsWithin)
        {
            //store position and change z position to 0 - needed to get the right "layer" of tiles
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            //get TileBase from that position
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }


    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        //fill this array with TileBases of the chosen type
        FillTiles(tileArray, type);
        //set the tiles on the tilemap
        tilemap.SetTilesBlock(area, tileArray);
    }


    private static void FillTiles(TileBase[] arr, TileType type)
    {
        //go through each tile and set it from TileBases dictionary
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement


    public void InitializeWithBuilding(GameObject building)
    {
        //calculate the position for the house; in the center (Vector3.zero),
        //with an offset (Vector3(.5f, .5f, 0f))
        Vector3 position = gridLayout.CellToLocalInterpolated(offset2);
        //initialize current building
        temp = Instantiate(building, position, Quaternion.identity).GetComponent<Building>();
        FollowBuilding();//占据网格提示
    }

    private void ClearArea()//将房屋所在的先前区域设置为空
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];//数组存储网格
        FillTiles(toClear, TileType.Empty);//
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    public void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])//遍历检查是否是白网格
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);//检测到其他类型的网格则说明不可放置，为红
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    #endregion

}

//网格种类
public enum TileType
{
    Empty, //非网格
    White, //空余
    Green, //可放置
    Red    //不可放置
}

*/



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    //singletone pattern
    public static GridBuildingSystem current;

    public GridLayout gridLayout; //Grid Object
    public Tilemap MainTilemap; //Main tilemap - for checking placement availability
    public Tilemap TempTilemap; //Temp tilemap - to indicate where the building is now

    //all tile types - white, green, red
    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    //to keep track of the current building
    private Building temp; //building itself
    private BoundsInt prevArea; //to clear the area it was standing on

    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //todo you can change the initialization method if you want to
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
    }

    private void Update()
    {
        //we don't have a building currently being placed
        if (!temp)
        {
            //nothing to do
            return;
        }

        //I chose Space bar to confirm house placement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //check if we can place here
            if (temp.CanBePlaced())
            {
                //yes, we can
                temp.Place();
            }
        }
        //Escape button to cancel the building placement
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            //clear the area the building was standing on
            ClearArea();
            //destroy the building object
            Destroy(temp.gameObject);
        }
    }

    #endregion

    #region Tilemap management

    /*
     * Gets an array of tiles from the tilemap
     *
     * UNITY'S METHOD GetTilesBlock CAUSES EDITOR CRASH
     * (at least in my case)
     * 
     * BoundsInt area - tiles come from this area;
     * it has a position on the tilemap and size (2, 2, 1) etc.
     * Tilemap tilemap - tilemap from which we get tiles
     *  @returns an array of TilesBase
     *
     * Use this method when we check if area under the house is available
     * (white TileBase means available)
     */
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        //go through each position from the area
        foreach (var v in area.allPositionsWithin)
        {
            //store position and change z position to 0 - needed to get the right "layer" of tiles
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            //get TileBase from that position
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    /*
     * Sets TileBases on a tilemap
     * BoundsInt area - tiles set on this area;
     * it has a position on the tilemap and size (2, 2, 1) etc.
     * TileType - which tiles to set (white, green or blue)
     * Tilemap tilemap - tilemap on which we set tiles
     */
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        //fill this array with TileBases of the chosen type
        FillTiles(tileArray, type);
        //set the tiles on the tilemap
        tilemap.SetTilesBlock(area, tileArray);
    }

    /*
     * Fills an array of tiles with the chosen TileType
     */
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        //go through each tile and set it from TileBases dictionary
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    /*
     * Initialize the system with a building prefab
     * GameObject building - prefab
     */
    public void InitializeWithBuilding(GameObject building)
    {
        //calculate the position for the house; in the center (Vector3.zero),
        //with an offset (Vector3(.5f, .5f, 0f))
        Vector3 position = gridLayout.CellToLocalInterpolated(new Vector3(.5f, .5f, 0f));
        //initialize current building
        temp = Instantiate(building, position, Quaternion.identity).GetComponent<Building>();
        //highlight the area under it
        FollowBuilding();
    }

    /*
     * Set previous area that the house was standing on to empty
     */
    private void ClearArea()
    {
        //create an array to store the tiles
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        //fill the array with empty tiles
        FillTiles(toClear, TileType.Empty);
        //set the tiles on the temporary tilemaps
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    /*
     * Highlight the area under the building (on the Temporary tilemap)
     */
    public void FollowBuilding()
    {
        //clear the previously highlighted area
        ClearArea();

        //calculate the position of the area
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        //save the area
        BoundsInt buildingArea = temp.area;

        //get the tiles from the MainTilemap to determine with what color to highlight it
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        //create an array to store the tiles
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            //white TileBase - tile available
            if (baseArray[i] == tileBases[TileType.White])
            {
                //placement possible, highlight tiles green
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                //not a white tile - placement not possible, highlight tiles red
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        //set the tiles on the temporary tilemap (highlight the area)
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    /*
     * Check if an area is available for placement
     * BoundsInt area - area to check
     */
    public bool CanTakeArea(BoundsInt area)
    {
        //get TileBases from the Main tilemap at this area
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        //check each TileType
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                //not white = not available
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    /*
     * Take the area for a building
     */
    public void TakeArea(BoundsInt area)
    {
        //clear the highlighted area under the building 
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        //set the tiles on the Main tilemap to indicate that the area is taken
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    #endregion
}

//types of tiles
/*
public enum TileType
{
   Empty, //empty
   White, //available
    Green, //can place
    Red    //can't place
}
*/