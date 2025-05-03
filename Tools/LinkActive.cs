using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkActive : MonoBehaviour
{
    [SerializeField] private List<GameObject> linkTargets;
    private void OnEnable()
    {
        // 当当前物体激活时，确保所有 linkTargets 也激活
        SyncTargets(true);
    }

    private void OnDisable()
    {
        // 当当前物体禁用时，确保所有 linkTargets 也禁用
        SyncTargets(false);
    }

    private void Start()
    {
        // 初始化时同步状态
        if (linkTargets != null && linkTargets.Count > 0)
        {
            // 根据第一个目标物体的状态同步当前物体
            gameObject.SetActive(linkTargets[0].activeSelf);
        }
    }

    private void SyncTargets(bool activeState)
    {
        if (linkTargets == null || linkTargets.Count == 0)
            return;

        // 遍历所有 linkTargets，设置它们的激活状态
        foreach (var target in linkTargets)
        {
            if (target != null && target.activeSelf != activeState)
            {
                target.SetActive(activeState);
            }
        }
    }
}
