using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// ���AudioManager�ĵ���
    /// </summary>
    public static AudioManager instance;

    [Header("��Ƶ����")]
    public AudioType[] AudioTypes;  // ��Ƶ��������,�����Ҫ���ŵ���Ƶ
    [Header("��Ƶ����")]
    public AudioMixer mixer;    // ��Ƶ�����

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);  // ��֤�л�����ʱ��������
    }

    private void OnEnable()
    {
        foreach (var type in AudioTypes)    // ����AudioTypes������г�ʼ��
        {
            type.Source = gameObject.AddComponent<AudioSource>();    // �ڵ�����AudioManager�Ľű���GameObject�����AudioSource(����)���

            type.Source.name = type.Name;    // ����AudioSource������
            type.Source.volume = type.Volume;    // ����
            type.Source.pitch = type.Pitch;    // ����
            type.Source.loop = type.Loop;    // �Ƿ�ѭ������
            type.Source.playOnAwake = type.PlayOnAwake;    // �Ƿ���Awakeʱ�Զ�����

            if (type.MixerGroup != null)      // ���������AudioMixerGroup
            {
                type.Source.outputAudioMixerGroup = type.MixerGroup;    // ����AudioMixerGroup
            }
        }
    }

    public void PlayBGM(string name)    // ������Ƶʱ����
    {

        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            type.Source.Stop();    // ֹͣ������Ƶ
        }

        foreach (AudioType type in AudioTypes) // ����AudioTypes����
        {
            if (type.Name == name) // ����ҵ�����name��Ӧ����Ƶ
            {
                type.Source.clip = type.Clip; // ������ƵClip
                type.Source.Play(); // ������Ƶ
                return;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");    // û�ҵ���Ƶʱ���������Ϣ
    }

    public void PlayBGM(AudioClip clip)    // ������Ƶʱ����
    {

        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            type.Source.Stop();    // ֹͣ������Ƶ
        }

        foreach (AudioType type in AudioTypes) // ����AudioTypes����
        {
            if (type.Clip == clip) // ����ҵ�����name��Ӧ����Ƶ
            {
                type.Source.clip = type.Clip; // ������ƵClip
                type.Source.Play(); // ������Ƶ
                return;
            }
        }

        Debug.LogError("û���ҵ�" + clip + "��Ƶ");    // û�ҵ���Ƶʱ���������Ϣ
    }

    public void PlayBirdsong(string name)
    {
        foreach (AudioType type in AudioTypes)
        {
            if (type.Name == name)
            {
                type.Source.loop = true; // ȷ��ѭ������
                type.Source.clip = type.Clip;
                type.Source.Play();
                return;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");
    }


    public void PlayFX(string name) // ������Чʱ����
    {
        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            if (type.Name == name)    // ����ҵ�����name��Ӧ����Ƶ
            {
                type.Source.PlayOneShot(type.Clip);    // ������Ч
                return;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ч");    // û�ҵ���Чʱ���������Ϣ
    }

    public void Pause(string name)    // ��ͣ��Ƶʱ����
    {
        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            if (type.Name == name)
            {
                type.Source.Pause();    // ��ͣ��Ƶ
                return;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");
    }

    public void Stop(string name)    // ֹͣ��Ƶʱ����
    {
        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            if (type.Name == name)
            {
                type.Source.Stop();    // ֹͣ��Ƶ
                return;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");
    }

    public void StopAll()    // ֹͣ������Ƶʱ����
    {
        foreach (AudioType type in AudioTypes)    // ����AudioTypes����
        {
            type.Source.Stop();    // ֹͣ��Ƶ   
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");
    }

    public bool IsPlaying(string name) // �ж���Ƶ�Ƿ����ڲ���
    {
        foreach (AudioType type in AudioTypes)
        {
            if (type.Name == name)
            {
                return type.Source.isPlaying;
            }
        }

        Debug.LogError("û���ҵ�" + name + "��Ƶ");
        return false;
    }



}