using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    private int XPNow;
    //private int Level;
    public static int Level { get; private set; }
    private int xpToNext;

    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject lvlWindowPrefab;

    private Image slider;
    private TextMeshProUGUI xpText;
    private TextMeshProUGUI lvlText;
    //private Image starImage;

    private static bool initialized;
    private static Dictionary<int, int> xpToNextLevel = new Dictionary<int, int>();
    private static Dictionary<int, int[]> lvlReward = new Dictionary<int, int[]>();

    private void Awake()
    {
        slider = levelPanel.transform.Find("LVSlider").GetComponent<Image>();
        xpText = levelPanel.transform.Find("Text_XP").GetComponent<TextMeshProUGUI>();
        //starImage = levelPanel.transform.Find("Star").GetComponent<Image>();
        lvlText = levelPanel.transform.Find("Text_LV").GetComponent<TextMeshProUGUI>();

        if (!initialized)
        {
            Initialize();
        }

        xpToNextLevel.TryGetValue(Level, out xpToNext);
    }

    
    private static void Initialize()
    {
        try
        {
            string path = "levelsXP";
            TextAsset textAsset = Resources.Load<TextAsset>(path);

            if (textAsset == null)
            {
                Debug.LogError($"Error: Failed to load CSV file at {path}");
                return;
            }

            string[] lines = textAsset.text.Split('\n');
            xpToNextLevel.Clear();
            lvlReward.Clear();

            for (int i = 1; i < lines.Length; i++)  // 遍历所有行
            {
                if (string.IsNullOrWhiteSpace(lines[i]))  // 跳过空行
                    continue;

                string[] columns = lines[i].Split(',');  //逗号分割（重要）

                if (columns.Length < 4)  // 确保至少有 4 列
                {
                    Debug.LogError($"Error: Malformed line {i} in CSV: {lines[i]}");
                    continue;
                }

                int lvl, xp, curr1, curr2;
                if (!int.TryParse(columns[0], out lvl) ||
                    !int.TryParse(columns[1], out xp) ||
                    !int.TryParse(columns[2], out curr1) ||
                    !int.TryParse(columns[3], out curr2))
                {
                    Debug.LogError($"Error: Failed to parse line {i} in CSV: {lines[i]}");
                    continue;
                }

                if (!xpToNextLevel.ContainsKey(lvl))
                {
                    xpToNextLevel.Add(lvl, xp);
                    lvlReward.Add(lvl, new[] { curr1, curr2 });
                    Debug.Log($"Added level {lvl} - XP: {xp}, Reward: {curr1}, {curr2}");
                }
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception in Initialize: {ex.Message}");
        }

        initialized = true;
    }

    /*
    private static void Initialize()
    {
        try
        {
            // path to the csv file
            string path = "levelsXP";
            
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            string[] lines = textAsset.text.Split('\n');
            
            xpToNextLevel = new Dictionary<int, int>(lines.Length - 1);
            
            for(int i = 1; i < lines.Length - 1; i++)
            {
                string[] columns = lines[i].Split(',');
                
                int lvl = -1;
                int xp = -1;
                int curr1 = -1;
                int curr2 = -1;
                
                int.TryParse(columns[0], out  lvl);
                int.TryParse(columns[1], out xp);
                int.TryParse(columns[2], out curr1);
                int.TryParse(columns[3], out curr2);

                if (lvl >= 0 && xp > 0)
                {
                    if (!xpToNextLevel.ContainsKey(lvl))
                    {
                        xpToNextLevel.Add(lvl, xp);
                        lvlReward.Add(lvl, new []{curr1, curr2});
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        initialized = true;
    }
    */
    private void Start()
    {
        EventManager.Instance.AddListener<XPAddedGameEvent>(OnXPAdded);
        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        float fill = (float) XPNow / xpToNext;
        //slider.value = fill;
        slider.fillAmount = fill;
        xpText.text = XPNow + "/" + xpToNext;
    }

    private void OnXPAdded(XPAddedGameEvent info)
    {
        XPNow += info.amount;
        
        UpdateUI();

        if (XPNow >= xpToNext)
        {
            Level++;
            LevelChangedGameEvent levelChange = new LevelChangedGameEvent(Level);
            EventManager.Instance.QueueEvent(levelChange);
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        XPNow -= xpToNext;
        xpToNext = xpToNextLevel[info.newLvl];
        lvlText.text = (info.newLvl + 1).ToString();
        UpdateUI();

        GameObject window = Instantiate(lvlWindowPrefab, GameManager.current.canvas.transform);
        AudioManager.instance.PlayFX("twinkle1");
        //initialize texts and images here

        window.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
        {
            Destroy(window);
        });

        //CurrencyChangeGameEvent currencyInfo =
        //    new CurrencyChangeGameEvent(lvlReward[info.newLvl][0], CurrencyType.Coins);
        //EventManager.Instance.QueueEvent(currencyInfo);

        //currencyInfo =
        //    new CurrencyChangeGameEvent(lvlReward[info.newLvl][1], CurrencyType.Coins);
        //EventManager.Instance.QueueEvent(currencyInfo);

        CurrencyChangeGameEvent coinsInfo =
            new CurrencyChangeGameEvent(lvlReward[info.newLvl][0], CurrencyType.Coins);
        EventManager.Instance.QueueEvent(coinsInfo);

        CurrencyChangeGameEvent crystalsInfo =
            new CurrencyChangeGameEvent(lvlReward[info.newLvl][1], CurrencyType.Crystals); 
        EventManager.Instance.QueueEvent(crystalsInfo);

    }
}
