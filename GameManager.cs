using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using LootLocker;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public GameObject canvas;

    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LoadManager loadManager;

    private void Awake()
    {
        //initialize fields
        current = this;
        //loadManager.LoadGame();
        //initialize
        //DownloadSaveFromCloud();
        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();

        System.Net.ServicePointManager.ServerCertificateValidationCallback =
    (sender, certificate, chain, sslPolicyErrors) => true;
        //访客登录
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                //Debug.LogError($"LootLocker session failed. Error: {response.errorData?.message}");
                Debug.LogError($"[LootLocker] Guest Session failed. StatusCode: {response.statusCode}, Message: {response.errorData?.message}");

                return;
            }

            Debug.Log("successfully started LootLocker session");
            //UploadSaveToCloud();
            DownloadSaveFromCloud();
        });
    }
   
    void Start()
    {


    }

    public void GetXP(int amount)
    {
        XPAddedGameEvent info = new XPAddedGameEvent(amount);
        EventManager.Instance.QueueEvent(info);


    }

    public void GetCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Coins);
        EventManager.Instance.QueueEvent(info);

    }

    public void GetCrystals(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Crystals);
        EventManager.Instance.QueueEvent(info);

    }


    public bool SpendCurrency(CurrencyType currencyType, int amount)
    {
        int currentAmount = CurrencySystem.CurrencyAmounts[currencyType];

        if (currentAmount >= amount)
        {
            CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-amount, currencyType);
            EventManager.Instance.QueueEvent(info);
            return true; // 扣钱成功
        }
        else
        {
            NotEnoughCurrencyGameEvent info = new NotEnoughCurrencyGameEvent(amount, currencyType);
            EventManager.Instance.QueueEvent(info);
            return false; // 扣钱失败
        }
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            UPSaveToCloud();
        }
    }

    private void OnApplicationQuit()
    {
        UPSaveToCloud();
    }

    public void UPSaveToCloud()//实现GameManager.current.UPSaveToCloud();
    {
        SaveManager.Instance.SaveGame();
        UploadOrUpdateSaveFile();
    }


    public IEnumerator UploadSaveToCloudAndWait()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveFile.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError("Save file not found.");
            yield break;
        }

        bool isDone = false;
        bool success = false;

        LootLockerSDKManager.UploadPlayerFile(filePath, "saveFile.json", false, (response) =>
        {
            success = response.success;
            isDone = true;
        });

        float timeout = 5f; // 最多等待 5 秒
        float timer = 0f;

        while (!isDone && timer < timeout)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (success)
            Debug.Log("Save file uploaded successfully before quit.");
        else
            Debug.LogWarning("Upload may not have completed before application quit.");
    }


    public void UploadOrUpdateSaveFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveFile.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError("Save file not found.");
            return;
        }

        LootLockerSDKManager.GetAllPlayerFiles((getResponse) =>
        {
            if (!getResponse.success)
            {
                Debug.LogError("Failed to retrieve player files.");
                return;
            }

            var existingFile = getResponse.items.FirstOrDefault(f => f.name == "saveFile.json");

            if (existingFile != null)
            {
                // 已存在 → 更新
                int playerFileID = existingFile.id;
                LootLockerSDKManager.UpdatePlayerFile(playerFileID, filePath, (updateResponse) =>
                {
                    if (updateResponse.success)
                    {
                        Debug.Log("Save file updated successfully.");
                    }
                    else
                    {
                        Debug.LogError("Failed to update save file.");
                    }
                });
            }
            else
            {
                // 不存在 → 创建
                LootLockerSDKManager.UploadPlayerFile(filePath, "saveFile.json", false, (uploadResponse) =>
                {
                    if (uploadResponse.success)
                    {
                        Debug.Log("Save file uploaded successfully (new).");
                    }
                    else
                    {
                        Debug.LogError("Failed to upload new save file.");
                    }
                });
            }
        });
    }

    /*
    public void UpdateSaveToCloud()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveFile.json");
        LootLockerSDKManager.UpdatePlayerFile(playerFileID, filePath, response =>
        {
            if (response.success)
            {
                Debug.Log("Successfully updated player file, url: " + response.url);
            }
            else
            {
                Debug.Log("Error updating player file");
            }
        });
    }
    */
    public void DownloadSaveFromCloud()
    {
        LootLockerSDKManager.GetAllPlayerFiles((response) =>
        {
            if (response.success)
            {
                var saveFile = response.items.LastOrDefault(f => f.name == "saveFile.json");
                if (saveFile != null)
                {
                    Debug.Log("Cloud save file URL: " + saveFile.url); // 打印 URL
                    StartCoroutine(DownloadAndSaveToLocal(saveFile.url));
                    Debug.Log("成功下载");

                }
                else
                {
                    Debug.Log("No save file found in cloud.");
                    //首次登录
                    if (CurrencySystem.CurrencyAmounts[CurrencyType.Coins] == 0)
                    {
                        GetCoins(200); 
                        Debug.Log("首次登录，赠送 200 Coins");
                        UISystem.current.loadingPanel.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve player files: ");
            }
        });
    }

    private IEnumerator DownloadAndLoadSaveFile(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            loadManager.LoadGame();
            Debug.Log("Save file downloaded and loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to download save file: " + www.error);
        }
    }


    private IEnumerator DownloadAndSaveToLocal(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.certificateHandler = new BypassCertificate(); 
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "saveFile.json");

            try
            {
                File.WriteAllText(filePath, www.downloadHandler.text);
                Debug.Log("Save file downloaded and saved locally.");

                loadManager.LoadGame(); // 使用你原有的从本地加载方法
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving downloaded save file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Failed to download save file: " + www.error);
        }
    }

    class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // 信任所有证书（仅测试用）
        }
    }

}
