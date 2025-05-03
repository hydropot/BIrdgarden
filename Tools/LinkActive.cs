using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkActive : MonoBehaviour
{
    [SerializeField] private List<GameObject> linkTargets;
    private void OnEnable()
    {
        // ����ǰ���弤��ʱ��ȷ������ linkTargets Ҳ����
        SyncTargets(true);
    }

    private void OnDisable()
    {
        // ����ǰ�������ʱ��ȷ������ linkTargets Ҳ����
        SyncTargets(false);
    }

    private void Start()
    {
        // ��ʼ��ʱͬ��״̬
        if (linkTargets != null && linkTargets.Count > 0)
        {
            // ���ݵ�һ��Ŀ�������״̬ͬ����ǰ����
            gameObject.SetActive(linkTargets[0].activeSelf);
        }
    }

    private void SyncTargets(bool activeState)
    {
        if (linkTargets == null || linkTargets.Count == 0)
            return;

        // �������� linkTargets���������ǵļ���״̬
        foreach (var target in linkTargets)
        {
            if (target != null && target.activeSelf != activeState)
            {
                target.SetActive(activeState);
            }
        }
    }
}
