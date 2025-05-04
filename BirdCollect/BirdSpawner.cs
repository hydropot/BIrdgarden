using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class BirdSpawner : MonoBehaviour
{
    public float environment=0;
    public float spawnIntervalMinutes = 30f;
    public int maxBirdType;

    private Dictionary<string, BirdSpawnConfig> spawnConfigs = new Dictionary<string, BirdSpawnConfig>();

    public static BirdSpawner current;
    private void Awake()
    {
        //initialize fields
        current = this;
    }
    public void InitBird()
    {
        LoadSpawnConfigs();
        SimulateOfflineSpawning();
    }

    void SimulateOfflineSpawning2()
    {
        string lastTimeStr = PlayerPrefs.GetString("LastCloseTime", null);
        if (string.IsNullOrEmpty(lastTimeStr))
        {
            Debug.Log("没有找到上次退出时间，跳过模拟生成");
            return;
        }

        DateTime lastTime;
        if (!DateTime.TryParse(lastTimeStr, out lastTime))
        {
            Debug.LogWarning("无法解析退出时间：" + lastTimeStr);
            return;
        }

        TimeSpan delta = GameExitTracker.OfflineTime.Value;
        int attemptCount = Mathf.FloorToInt((float)delta.TotalMinutes / spawnIntervalMinutes);
        Debug.Log($"离线时间 {delta.TotalMinutes:F1} 分钟，可生成尝试 {attemptCount} 次");


        if (attemptCount <= 0) return;

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("item");
        List<Transform> availableSeats = new List<Transform>();

        foreach (GameObject item in allItems)
        {
            Transform seat = item.transform.Find("birdseat");
            if (seat != null && seat.childCount == 0)
                availableSeats.Add(seat);
        }

        // 为每个 item 最多尝试一次
        foreach (Transform seat in availableSeats)
        {
            GameObject item = seat.parent.gameObject;
            string pureName = item.name.Replace("(Clone)", "").Trim();

            if (!spawnConfigs.TryGetValue(pureName, out var config))
                continue;

            for (int i = 0; i < attemptCount; i++)
            {
                float adjustedFailChance = Mathf.Max(0f, config.failChance - environment);
                float failRoll = UnityEngine.Random.Range(0f, 1f);
                if (failRoll < adjustedFailChance)
                {
                    Debug.Log($"{item.name} 第 {i + 1} 次尝试失败（原 fail={config.failChance}, 环境修正后={adjustedFailChance:F2}）");
                    continue;
                }

                int birdId = SelectBirdByWeight(config.birdWeights);
                if (birdId == -1) continue;

                string path = $"Prefab/Bird/bird{birdId}";
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab != null)
                {
                    GameObject bird = Instantiate(prefab, seat.position, seat.rotation, seat);
                    bird.name = prefab.name;
                    Debug.Log($"在 {item.name} 的 birdseat 离线生成 bird{birdId}");
                }
                break; // seat 成功生成一次就退出尝试
            }
        }


        /*
            // 为每个 item 最多尝试一次
            foreach (Transform seat in availableSeats)
            {
                GameObject item = seat.parent.gameObject;
                string pureName = item.name.Replace("(Clone)", "").Trim();

                if (!spawnConfigs.TryGetValue(pureName, out var config))
                    continue;

                for (int i = 0; i < attemptCount; i++)
                {
                    float adjustedFailChance = Mathf.Max(0f, config.failChance - environment);
                    float failRoll = UnityEngine.Random.Range(0f, 1f);
                    if (failRoll < adjustedFailChance)
                    {
                        Debug.Log($"{item.name} 第 {i + 1} 次尝试失败（原 fail={config.failChance}, 环境修正后={adjustedFailChance:F2}）");
                        continue;
                    }

                    int birdId = SelectBirdByWeight(config.birdWeights);
                    if (birdId == -1) continue;

                    string path = $"Prefab/Bird/bird{birdId}";
                    GameObject prefab = Resources.Load<GameObject>(path);
                    if (prefab != null)
                    {
                        GameObject bird = Instantiate(prefab, seat.position, seat.rotation, seat);
                        bird.name = prefab.name;
                        Debug.Log($"在 {item.name} 离线生成 bird{birdId}");
                    }
                    break; // 成功生成一次就退出尝试
                }
            }
        */
    }

    void SimulateOfflineSpawning()
    {
        if (!GameExitTracker.OfflineTime.HasValue)
        {
            Debug.Log("离线时长尚未准备好，跳过模拟生成");
            return;
        }

        TimeSpan delta = GameExitTracker.OfflineTime.Value;
        int attemptCount = Mathf.FloorToInt((float)delta.TotalMinutes / spawnIntervalMinutes);
        Debug.Log($"离线时间 {delta.TotalMinutes:F1} 分钟，可生成尝试 {attemptCount} 次");

        if (attemptCount <= 0) return;

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("item");

        foreach (GameObject item in allItems)
        {
            string pureName = item.name.Replace("(Clone)", "").Trim();
            if (!spawnConfigs.TryGetValue(pureName, out var config)) continue;

            Transform[] seats = item.GetComponentsInChildren<Transform>()
                                    .Where(t => t.name == "birdseat" && t.childCount == 0)
                                    .ToArray();

            foreach (Transform seat in seats)
            {
                for (int i = 0; i < attemptCount; i++)
                {
                    float adjustedFailChance = Mathf.Max(0f, config.failChance - environment);
                    float failRoll = UnityEngine.Random.Range(0f, 1f);
                    if (failRoll < adjustedFailChance)
                    {
                        continue;
                    }

                    int birdId = SelectBirdByWeight(config.birdWeights);
                    if (birdId == -1) continue;

                    string path = $"Prefab/Bird/bird{birdId}";
                    GameObject prefab = Resources.Load<GameObject>(path);
                    if (prefab != null)
                    {
                        GameObject bird = Instantiate(prefab, seat.position, seat.rotation, seat);
                        bird.name = prefab.name;
                        Debug.Log($"在 {item.name}/{seat.name} 离线生成 bird{birdId}");

                        // 生成硬币
                        GameObject coinPrefab = Resources.Load<GameObject>("Prefab/Currency/coins");
                        GameObject coin = Instantiate(coinPrefab, seat.position + Vector3.left * (UnityEngine.Random.Range(-0.5f, 0.5f)), seat.rotation, seat);
                        coin.name = coinPrefab.name;
                    }
                    break; // 成功生成一次就退出尝试
                }
            }
        }
    }



    int SelectBirdByWeight(Dictionary<int, float> weights)
    {
        float total = weights.Values.Sum();
        if (total <= 0) return -1;

        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var pair in weights)
        {
            cumulative += pair.Value;
            if (roll <= cumulative)
                return pair.Key;
        }
        return -1;
    }

    void LoadSpawnConfigs()
    {
        TextAsset csv = Resources.Load<TextAsset>("spawner");
        if (csv == null)
        {
            Debug.LogError("无法读取 spawner.csv");
            return;
        }

        string[] lines = csv.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] parts = line.Split(',');
            if (parts.Length < 2) continue;

            string itemName = parts[0].Trim();
            BirdSpawnConfig config = new BirdSpawnConfig();
            if (!float.TryParse(parts[1], out config.failChance)) continue;

            for (int j = 2; j < parts.Length; j++)
            {
                if (float.TryParse(parts[j], out float weight) && weight > 0)
                {
                    config.birdWeights[j - 1] = weight;
                }
            }

            spawnConfigs[itemName] = config;
        }

        //Debug.Log("加载鸟生成配置完成，共 " + spawnConfigs.Count + " 项");
    }
}
