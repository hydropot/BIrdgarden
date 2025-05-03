using UnityEngine;

public class PickCoins : MonoBehaviour
{
    private void OnMouseDown()
    {
        // ���� GameManager ��ȡ���
        if (GameManager.current != null)
        {
            AudioManager.instance.PlayFX("pop4");
            GameManager.current.GetCoins(10);
            GameManager.current.GetXP(10);
            // ���ٽ�Ҷ���
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("GameManager δ��ʼ�����޷���ý��");
        }
    }
}
