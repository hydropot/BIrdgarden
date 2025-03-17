using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    private Vector3 origin;
    public BoundsInt area;

    private float pressTime = 0f;
    private bool isPressing = false;

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

        Placed = true;
        origin = transform.position;

        BuildingSystem.current.TakeArea(areaTemp);
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

            ShopManager.current.ShopButton_Click();
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
        DetectLongPress();
    }


    private void DetectLongPress()
    {
        // 只对已放置的物体启用长按检测
        if (!Placed) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        if (Input.GetMouseButtonDown(0))
        {
            // 判断鼠标是否点击在该物体上
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

            if (pressTime >= 1f)
            {
                // 长按成功，进入移动模式
                isPressing = false;

                // 清除之前占用的 Tile 区域
                Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
                BoundsInt areaTemp = area;
                areaTemp.position = positionInt;
                //BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTilemap);
                BuildingSystem.current.ReleaseArea(areaTemp);


                Placed = false; // 标记为未放置状态
                gameObject.AddComponent<ObjectDrag>();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPressing = false;
        }
    }
}
