using UnityEngine;
using System.IO;

public class SaveFilePathDebugger : MonoBehaviour
{
    private void Start()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "saveFile.json");
        Debug.Log("Easy Save �ļ�����·����: " + fullPath);

        if (File.Exists(fullPath))
        {
            Debug.Log("�ļ����ڣ�");
        }
        else
        {
            Debug.LogWarning("�ļ������ڣ�");
        }
    }
}
