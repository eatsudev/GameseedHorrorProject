using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoudnessFrom_Microphone : MonoBehaviour
{
    public static LoudnessFrom_Microphone instance;
    public Audio_Detection detection;

    public Slider loudnessSlider;              // UI Slider for loudness display
    public Image fillImage;                    // The fill image of the slider
    public Image background;                   // The background image of the slider

    public float opacityThreshold = 0.1f;      // The threshold below which the slider will fade out
    public float fadeSpeed = 2f;               // Speed at which opacity changes

    private float currOpacity = 1f;            // Current opacity of the slider images
    private Color green = Color.green;
    private Color yellow = Color.yellow;
    private Color red = Color.red;


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

        // Ensure slider and images are set
        loudnessSlider.minValue = minLoudness;
        loudnessSlider.maxValue = maxLoudness;
    }


    void Update()
    {
        currLoudness = detection.GetLoudnessFromMic() * loudnessSensibility;

        if(currLoudness < threshold)
        {
            currLoudness = 0;
        }

        loudnessSlider.value = currLoudness;

        // Update slider color (lerp from green to yellow to red)
        UpdateSliderColor(currLoudness);

        // Adjust opacity based on loudness
        UpdateSliderOpacity(currLoudness);
    }

    // Update the fill color based on loudness value
    private void UpdateSliderColor(float loudness)
    {
        if (loudness < maxLoudness / 2f)
        {
            // Lerp between green and yellow for the first half of the loudness range
            fillImage.color = Color.Lerp(green, yellow, loudness / (maxLoudness / 2f));
        }
        else
        {
            // Lerp between yellow and red for the second half
            fillImage.color = Color.Lerp(yellow, red, (loudness - maxLoudness / 2f) / (maxLoudness / 2f));
        }
    }

    // Update the slider and background opacity based on loudness
    private void UpdateSliderOpacity(float loudness)
    {
        if (loudness < opacityThreshold)
        {
            currOpacity = Mathf.Lerp(currOpacity, 0f, Time.deltaTime * fadeSpeed);
        }
        else
        {
            currOpacity = Mathf.Lerp(currOpacity, 1f, Time.deltaTime * fadeSpeed);
        }

        // Apply the opacity to both the fill and the background
        SetImageOpacity(fillImage, currOpacity);
        SetImageOpacity(background, currOpacity);
    }

    // Helper function to change the opacity of an image
    private void SetImageOpacity(Image image, float opacity)
    {
        Color color = image.color;
        color.a = opacity;
        image.color = color;
    }

    public float Loudness()
    {
        return currLoudness;
    }
}
