using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 startPos;
    private float deltaX, deltaY;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = Input.mousePosition;
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = new Vector3(mousePos.x - deltaX, mousePos.y - deltaY);

        transform.position = new Vector3(targetPos.x, targetPos.y, 0);

        // 动态调整排序层，y 越小，sortingOrder 越大，显示在上方
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

        // 实时高亮提示显示在 TempTilemap
        if (BuildingSystem.current.TempTilemap != null)
        {
            BuildingSystem.current.FollowObject(GetComponent<PlaceableObject>());
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PlaceableObject obj = GetComponent<PlaceableObject>();

            // 吸附到最近网格中心
            Vector3Int cellPos = BuildingSystem.current.gridLayout.WorldToCell(transform.position);
            transform.position = BuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));

            // 再次更新排序层以确保吸附后正确遮挡
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);

            // 清除高亮区域（仅清TempTilemap）
            BuildingSystem.current.ClearArea(obj.area, BuildingSystem.current.TempTilemap);

            obj.CheckPlacement();
            Destroy(this);
        }
    }
}
