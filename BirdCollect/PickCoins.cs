using UnityEngine;

public class PickCoins : MonoBehaviour
{
    private void OnMouseDown()
    {
        // 调用 GameManager 获取金币
        if (GameManager.current != null)
        {
            AudioManager.instance.PlayFX("pop4");
            GameManager.current.GetCoins(10);
            GameManager.current.GetXP(10);
            // 销毁金币对象
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("GameManager 未初始化，无法获得金币");
        }
    }
}
