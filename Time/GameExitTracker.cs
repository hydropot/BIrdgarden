using UnityEngine;
using System;

public class GameExitTracker : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        string now = DateTime.Now.ToString("O"); // ISO 8601 格式
        PlayerPrefs.SetString("LastCloseTime", now);
        PlayerPrefs.Save();
        Debug.Log("记录退出时间：" + now);
    }
}
