using UnityEngine;
using System.IO;

public class SaveFilePathDebugger : MonoBehaviour
{
    private void Start()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "saveFile.json");
        Debug.Log("Easy Save 文件完整路径是: " + fullPath);

        if (File.Exists(fullPath))
        {
            Debug.Log("文件存在！");
        }
        else
        {
            Debug.LogWarning("文件不存在！");
        }
    }
}
