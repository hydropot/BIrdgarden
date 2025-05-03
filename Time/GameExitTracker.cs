using UnityEngine;
using System;

public class GameExitTracker : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        string now = DateTime.Now.ToString("O"); // ISO 8601 ��ʽ
        PlayerPrefs.SetString("LastCloseTime", now);
        PlayerPrefs.Save();
        Debug.Log("��¼�˳�ʱ�䣺" + now);
    }
}
