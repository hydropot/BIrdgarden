using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 获得AudioManager的单例
    /// </summary>
    public static AudioManager instance;

    [Header("音频类型")]
    public AudioType[] AudioTypes;  // 音频类型数组,存放需要播放的音频
    [Header("音频设置")]
    public AudioMixer mixer;    // 音频混合器

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);  // 保证切换场景时不被销毁
    }

    private void OnEnable()
    {
        foreach (var type in AudioTypes)    // 遍历AudioTypes数组进行初始化
        {
            type.Source = gameObject.AddComponent<AudioSource>();    // 在调用了AudioManager的脚本的GameObject上添加AudioSource(喇叭)组件

            type.Source.name = type.Name;    // 设置AudioSource的名字
            type.Source.volume = type.Volume;    // 音量
            type.Source.pitch = type.Pitch;    // 音调
            type.Source.loop = type.Loop;    // 是否循环播放
            type.Source.playOnAwake = type.PlayOnAwake;    // 是否在Awake时自动播放

            if (type.MixerGroup != null)      // 如果有设置AudioMixerGroup
            {
                type.Source.outputAudioMixerGroup = type.MixerGroup;    // 设置AudioMixerGroup
            }
        }
    }

    public void PlayBGM(string name)    // 播放音频时调用
    {

        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            type.Source.Stop();    // 停止所有音频
        }

        foreach (AudioType type in AudioTypes) // 遍历AudioTypes数组
        {
            if (type.Name == name) // 如果找到名字name对应的音频
            {
                type.Source.clip = type.Clip; // 设置音频Clip
                type.Source.Play(); // 播放音频
                return;
            }
        }

        Debug.LogError("没有找到" + name + "音频");    // 没找到音频时输出错误信息
    }

    public void PlayBGM(AudioClip clip)    // 播放音频时调用
    {

        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            type.Source.Stop();    // 停止所有音频
        }

        foreach (AudioType type in AudioTypes) // 遍历AudioTypes数组
        {
            if (type.Clip == clip) // 如果找到名字name对应的音频
            {
                type.Source.clip = type.Clip; // 设置音频Clip
                type.Source.Play(); // 播放音频
                return;
            }
        }

        Debug.LogError("没有找到" + clip + "音频");    // 没找到音频时输出错误信息
    }

    public void PlayBirdsong(string name)
    {
        foreach (AudioType type in AudioTypes)
        {
            if (type.Name == name)
            {
                type.Source.loop = true; // 确保循环开启
                type.Source.clip = type.Clip;
                type.Source.Play();
                return;
            }
        }

        Debug.LogError("没有找到" + name + "音频");
    }


    public void PlayFX(string name) // 播放音效时调用
    {
        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            if (type.Name == name)    // 如果找到名字name对应的音频
            {
                type.Source.PlayOneShot(type.Clip);    // 播放音效
                return;
            }
        }

        Debug.LogError("没有找到" + name + "音效");    // 没找到音效时输出错误信息
    }

    public void Pause(string name)    // 暂停音频时调用
    {
        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            if (type.Name == name)
            {
                type.Source.Pause();    // 暂停音频
                return;
            }
        }

        Debug.LogError("没有找到" + name + "音频");
    }

    public void Stop(string name)    // 停止音频时调用
    {
        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            if (type.Name == name)
            {
                type.Source.Stop();    // 停止音频
                return;
            }
        }

        Debug.LogError("没有找到" + name + "音频");
    }

    public void StopAll()    // 停止所有音频时调用
    {
        foreach (AudioType type in AudioTypes)    // 遍历AudioTypes数组
        {
            type.Source.Stop();    // 停止音频   
        }

        Debug.LogError("没有找到" + name + "音频");
    }

    public bool IsPlaying(string name) // 判断音频是否正在播放
    {
        foreach (AudioType type in AudioTypes)
        {
            if (type.Name == name)
            {
                return type.Source.isPlaying;
            }
        }

        Debug.LogError("没有找到" + name + "音频");
        return false;
    }



}