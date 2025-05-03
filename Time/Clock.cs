using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    [Header("UI 组件")]
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI dayText;

    [Header("指针设置")]
    public RectTransform sun;
    public RectTransform pointer;
    public Image sunImage;
    public Sprite sunSprite;
    public Sprite moonSprite;

    private float degreesPerMinute = 180f / 1440f; // 每分钟 0.125 度
    private float nextUpdateTime = 0f;

    void Start()
    {
        UpdateClockVisuals();
        nextUpdateTime = GetNextMinuteTime();
    }

    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateClockVisuals();
            nextUpdateTime = GetNextMinuteTime();
        }
    }

    void UpdateClockVisuals()
    {
        // 更新时间文本
        DateTime now = DateTime.Now;
        monthText.text = now.Month.ToString() + "月";
        dayText.text = now.Day.ToString("00");

        // 计算指针角度
        int totalMinutes = now.Hour * 60 + now.Minute;
        float angle = totalMinutes * degreesPerMinute;
        sun.localRotation = Quaternion.Euler(0f, 0f, -angle); // 逆时针旋转
        pointer.localRotation = Quaternion.Euler(0f, 0f, -angle); // 逆时针旋转

        // 设置图标
        if (now.Hour >= 6 && now.Hour < 18)
        {
            sunImage.sprite = sunSprite;
        }
        else
        {
            sunImage.sprite = moonSprite;
        }
    }

    float GetNextMinuteTime()
    {
        DateTime now = DateTime.Now;
        DateTime nextMinute = now.AddMinutes(1);
        nextMinute = new DateTime(nextMinute.Year, nextMinute.Month, nextMinute.Day, nextMinute.Hour, nextMinute.Minute, 0);
        return (float)(nextMinute - DateTime.Now).TotalSeconds + Time.time;
    }
}
