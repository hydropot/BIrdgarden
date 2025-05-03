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
        sliderFX.value = 1f;
        sliderBGM.value = 1f;
        sliderBirdsong.value = 1f;

        sliderFX.onValueChanged.AddListener(SetFXVolume);
        sliderBGM.onValueChanged.AddListener(SetBGMVolume);
        sliderBirdsong.onValueChanged.AddListener(SetBirdsongVolume);
    }

    public void SetFXVolume(float value)
    {
        audioMixer.SetFloat("FXVolume", LinearToDecibel(value));
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", LinearToDecibel(value));
    }

    public void SetBirdsongVolume(float value)
    {
        audioMixer.SetFloat("BirdsongVolume", LinearToDecibel(value));
    }

    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f)
            return -80f; // 静音
        return Mathf.Log10(linear) * 20f;
    }
}
