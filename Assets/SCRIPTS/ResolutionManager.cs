using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionManager : MonoBehaviour
{
    public TextMeshProUGUI resolutionText;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;

    void Start()
    {
        // 1. Get all resolutions supported by the monitor
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        // 2. Filter out resolutions with different refresh rates to avoid duplicates
        double currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // 3. Find our current resolution in the list so we start at the right spot
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        UpdateResText();
    }

    public void NextResolution()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= filteredResolutions.Count) 
            currentResolutionIndex = 0; // Loop back to start
            
        UpdateResText();
    }

    public void PreviousResolution()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0) 
            currentResolutionIndex = filteredResolutions.Count - 1; // Loop to end
            
        UpdateResText();
    }

    void UpdateResText()
    {
        Resolution res = filteredResolutions[currentResolutionIndex];
        resolutionText.text = res.width + " X " + res.height;
    }

    // Call this when the user clicks a "Apply" button or automatically
    public void ApplyResolution()
    {
        Resolution res = filteredResolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}