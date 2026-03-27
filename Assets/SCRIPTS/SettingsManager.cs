using UnityEngine;
using UnityEngine.SceneManagement; // Needed to return to Menu scene if separate

public class SettingsManager : MonoBehaviour
{
    public GameObject videoPanel;
    public GameObject audioPanel;
    public GameObject settingsCanvas; // The whole settings UI
    public GameObject mainMenuCanvas; // Your main menu UI

    void Start()
    {
        // Start with Video showing by default
        ShowVideo();
    }

    public void ShowVideo()
    {
        videoPanel.SetActive(true);
        audioPanel.SetActive(false);
    }

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        videoPanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        // Option A: If Settings is a different Panel in the same scene
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);

        // Option B: If Settings is a different Scene (uncomment below if so)
        // SceneManager.LoadScene("MainMenu"); 
    }
}