using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public AudioMixer myMixer;
    public string parameterName; // Enter "MasterVolume", "MusicVolume", or "SFXVolume"
    public Slider volumeSlider;
    public TextMeshProUGUI percentageText;

    void Start()
    {
        // Load saved volume (default to 1.0 = 100%)
        float savedVolume = PlayerPrefs.GetFloat(parameterName, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float sliderValue)
    {
        // 1. Convert 0-1 slider value to decibels (-80 to 0)
        float dB = (sliderValue <= 0.0001f) ? -80f : Mathf.Log10(sliderValue) * 20f;
        myMixer.SetFloat(parameterName, dB);

        // 2. Update percentage text
        int percentage = Mathf.RoundToInt(sliderValue * 100);
        percentageText.text = percentage.ToString() + "%";

        // 3. Save the setting for next time the game opens
        PlayerPrefs.SetFloat(parameterName, sliderValue);
    }
}