using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentItem : MonoBehaviour
{
    public float environment = 0.1f;

    private void OnEnable()
    {
        BirdSpawner spawner = FindObjectOfType<BirdSpawner>();
        if (spawner != null)
        {
            spawner.environment += environment;
            Debug.Log($"{gameObject.name} 增加环境值 +{environment}，当前环境 = {spawner.environment:F2}");
        }
        else
        {
            Debug.LogWarning("找不到 BirdSpawner，无法添加环境影响");
        }
    }
}
