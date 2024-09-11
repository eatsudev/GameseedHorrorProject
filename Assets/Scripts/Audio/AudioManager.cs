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
    public Button saveButton;
    public Button discardButton;
    public Button resetButton;

    private float masterValue;
    private float bgmValue;
    private float sfxValue;

    private float defaultVolume = 0.75f;
    private void Start()
    {
        LoadVolumeSettings();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        saveButton.onClick.AddListener(SaveVolumeSettings);
        discardButton.onClick.AddListener(DiscardVolumeSettings);
        resetButton.onClick.AddListener(ResetVolumeSettings);
    }

    private void LoadVolumeSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", defaultVolume);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", defaultVolume);

        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", ValueToVolume(volume, 80));
        //PlayerPrefs.SetFloat("MasterVolume", volume);

        masterValue = volume;
    }

    public void SetBGMVolume(float volume)
    {
        masterMixer.SetFloat("BGM", ValueToVolume(volume, masterValue * 80));
        //PlayerPrefs.SetFloat("BGM", volume);

        bgmValue = volume;
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFX", ValueToVolume(volume, masterValue * 80));
        //PlayerPrefs.SetFloat("SFX", volume);

        sfxValue = volume;
    }
    private float ValueToVolume(float value, float maxVolume)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * maxVolume;
    }   

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Volume Settings Saved");
    }

    public void DiscardVolumeSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", defaultVolume);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);

        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);
        Debug.Log("Volume settings discarded");
    }

    public void ResetVolumeSettings()
    {
        masterSlider.value = defaultVolume;
        bgmSlider.value = defaultVolume;
        sfxSlider.value = defaultVolume;

        SetMasterVolume(defaultVolume);
        SetBGMVolume(defaultVolume);
        SetSFXVolume(defaultVolume);
        Debug.Log("Volume settings has been reset to default");
    }
}
