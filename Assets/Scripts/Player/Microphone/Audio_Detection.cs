using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Audio_Detection : MonoBehaviour
{
    public static Audio_Detection Instance;

    public bool isUsingMic = true;
    public int sampleWindow = 64;

    private AudioClip micClip;
    public TMP_Dropdown micDropdown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (isUsingMic)
        {
            PopulateDropdownWithMics();
            MicToAudioClip(Microphone.devices[0]);
        }
        micDropdown.onValueChanged.AddListener(delegate { OnMicDropdownChanged(); });
    }

    private void PopulateDropdownWithMics()
    {
        micDropdown.ClearOptions();
        List<string> options = new List<string>(Microphone.devices);

        if (options.Count > 0)
        {
            micDropdown.AddOptions(options);
        }
        else
        {
            options.Add("No Microphone Available");
            micDropdown.AddOptions(options);
            micDropdown.interactable = false;
        }
    }
    public void MicToAudioClip(string micName)
    {
        if (Microphone.devices.Length > 0)
        {
            if(micClip != null)
            {
                Microphone.End(micName);
            }

            micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);

            Debug.Log(micName);
            Debug.Log(Microphone.devices);
        }
        else
        {
            Debug.LogWarning("No Microphones Detected.");
        }
        
    }

    public void OnMicDropdownChanged()
    {
        string selectedMic = micDropdown.options[micDropdown.value].text;
        MicToAudioClip(selectedMic);
    }

    public void SelectMic(string newMicName)
    {
        micClip = Microphone.Start(newMicName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
    }
    public float GetLoudnessFromClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if(startPosition < 0)
        {
            startPosition = 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //Compute Loudness
        float totalLoudness = 0f;

        for(int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        Debug.Log(totalLoudness);
        return totalLoudness / sampleWindow;
    }
}
