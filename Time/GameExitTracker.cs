using UnityEngine;
using System;

public class GameExitTracker : MonoBehaviour
{
    private bool hasStarted = false;
    private const string LastCloseTimeKey = "LastCloseTime";

    public static TimeSpan? OfflineTime { get; private set; }

    private void Start()
    {
        hasStarted = true;

        // 读取上次退出时间
        if (PlayerPrefs.HasKey(LastCloseTimeKey))
        {
            string lastClose = PlayerPrefs.GetString(LastCloseTimeKey);
            if (DateTime.TryParse(lastClose, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastCloseTime))
            {
                OfflineTime = DateTime.Now - lastCloseTime;
                Debug.Log($"上次退出时间：{lastCloseTime:O}，离线时长：{OfflineTime}");
            }


        }
    }
    private void OnApplicationQuit()
    {
        RecordExitTime("OnApplicationQuit");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!hasStarted) return; // 避免启动时误判
        if (pauseStatus)
        {
            RecordExitTime("OnApplicationPause");
        }
    }
    private void RecordExitTime(string source)
    {
        string now = DateTime.Now.ToString("O"); // ISO 8601 格式，跨平台友好
        PlayerPrefs.SetString(LastCloseTimeKey, now);
        PlayerPrefs.Save();
        Debug.Log($"[{source}] 记录退出时间：{now}");
    }
}
