using UnityEngine;
using UnityEngine.UI;

public class FullScreenMode : MonoBehaviour
{
    // This function will be called by your Toggle
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // Debug to console to make sure it's working
        Debug.Log("Fullscreen is now: " + isFullscreen);
    }
}
