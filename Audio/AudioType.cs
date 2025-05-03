using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]	//在Inspector窗口中可见
public class AudioType  //音频类型
{
    [HideInInspector]
    public AudioSource Source;  //音频源(在Inspector中隐藏)
    public AudioClip Clip;  //音频片段
    public AudioMixerGroup MixerGroup;  //音频混音组

    public string Name;  //音频名称

    [Range(0f, 1f)]
    public float Volume;    //音量(滑动条)
    [Range(0.1f, 5f)]
    public float Pitch=1;    //音调(滑动条)
    public bool Loop;    //是否循环播放
    public bool PlayOnAwake;    //是否在Awake时自动播放
}
