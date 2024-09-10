using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private float masterValue;
    private float bgmValue;
    private float sfxValue;
    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 0.75f);
        
        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", ValueToVolume(volume, 80));
        PlayerPrefs.SetFloat("MasterVolume", volume);

        masterValue = volume;
    }

    public void SetBGMVolume(float volume)
    {
        masterMixer.SetFloat("BGM", ValueToVolume(volume, masterValue * 80));
        PlayerPrefs.SetFloat("BGM", volume);

        bgmValue = volume;
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFX", ValueToVolume(volume, masterValue * 80));
        PlayerPrefs.SetFloat("SFX", volume);

        sfxValue = volume;
    }
    private float ValueToVolume(float value, float maxVolume)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * maxVolume;
    }   
}
