using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Detection : MonoBehaviour
{
    public static Audio_Detection Instance;

    public bool isUsingMic = true;
    public int sampleWindow = 64;

    private AudioClip micClip;

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
            MicToAudioClip();
        }
    }
    public void MicToAudioClip()
    {
        string micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);

        Debug.Log(micName);
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

        return totalLoudness / sampleWindow;
    }
}
