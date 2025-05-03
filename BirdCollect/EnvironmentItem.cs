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
            Debug.Log($"{gameObject.name} ���ӻ���ֵ +{environment}����ǰ���� = {spawner.environment:F2}");
        }
        else
        {
            Debug.LogWarning("�Ҳ��� BirdSpawner���޷���ӻ���Ӱ��");
        }
    }
}
