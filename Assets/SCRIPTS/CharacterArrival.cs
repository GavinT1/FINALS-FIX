using UnityEngine;

public class CharacterArrival : MonoBehaviour
{
    public GameObject dialoguePanel;
    public float delayBeforeTalk = 0.5f; // Add a tiny pause for realism

    public void OnCharacterStopped()
    {
        if (dialoguePanel != null)
        {
            // This waits for 'delayBeforeTalk' seconds then runs the function below
            Invoke("ShowDialogue", delayBeforeTalk);
        }
    }

    void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
    }
}