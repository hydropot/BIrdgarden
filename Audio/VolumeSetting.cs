using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider sliderFX;
    public Slider sliderBGM;
    public Slider sliderBirdsong;

    void Start()
    {
        // 设置初始值（线性范围：0.0001f 到 1f）
        float fxVolume = PlayerPrefs.GetFloat("Volume_FX", 1f);
        float bgmVolume = PlayerPrefs.GetFloat("Volume_BGM", 1f);
        float birdsongVolume = PlayerPrefs.GetFloat("Volume_Birdsong", 1f);
        sliderFX.value = fxVolume;
        sliderBGM.value = bgmVolume;
        sliderBirdsong.value = birdsongVolume;

        sliderFX.onValueChanged.AddListener(SetFXVolume);
        sliderBGM.onValueChanged.AddListener(SetBGMVolume);
        sliderBirdsong.onValueChanged.AddListener(SetBirdsongVolume);
    }

    public void SetFXVolume(float value)
    {
        audioMixer.SetFloat("FXVolume", LinearToDecibel(value));
        PlayerPrefs.SetFloat("Volume_FX", value);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", LinearToDecibel(value));
        PlayerPrefs.SetFloat("Volume_BGM", value);
    }

    public void SetBirdsongVolume(float value)
    {
        audioMixer.SetFloat("BirdsongVolume", LinearToDecibel(value));
        PlayerPrefs.SetFloat("Volume_Birdsong", value);
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f; // 静音
        return Mathf.Log10(linear) * 20f;
    }
}
