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

        // ��ȡ�ϴ��˳�ʱ��
        if (PlayerPrefs.HasKey(LastCloseTimeKey))
        {
            string lastClose = PlayerPrefs.GetString(LastCloseTimeKey);
            if (DateTime.TryParse(lastClose, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastCloseTime))
            {
                OfflineTime = DateTime.Now - lastCloseTime;
                Debug.Log($"�ϴ��˳�ʱ�䣺{lastCloseTime:O}������ʱ����{OfflineTime}");
            }


        }
    }
    private void OnApplicationQuit()
    {
        RecordExitTime("OnApplicationQuit");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!hasStarted) return; // ��������ʱ����
        if (pauseStatus)
        {
            RecordExitTime("OnApplicationPause");
        }
    }
    private void RecordExitTime(string source)
    {
        string now = DateTime.Now.ToString("O"); // ISO 8601 ��ʽ����ƽ̨�Ѻ�
        PlayerPrefs.SetString(LastCloseTimeKey, now);
        PlayerPrefs.Save();
        Debug.Log($"[{source}] ��¼�˳�ʱ�䣺{now}");
    }
}
