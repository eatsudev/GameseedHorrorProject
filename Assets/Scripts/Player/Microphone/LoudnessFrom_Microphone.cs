using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudnessFrom_Microphone : MonoBehaviour
{
    public static LoudnessFrom_Microphone instance;
    public Audio_Detection detection;
    private AudioSource audioSource;

    public float minLoudness = 0f;
    public float maxLoudness = 5f;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    private float currLoudness;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        detection = GetComponent<Audio_Detection>();
    }


    void Update()
    {
        currLoudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if(currLoudness < threshold)
        {
            currLoudness = 0;
        }

        //Debug.Log(Mathf.Clamp(currLoudness, minLoudness, maxLoudness));
    }

    public float Loudness()
    {
        return currLoudness;
    }
}
