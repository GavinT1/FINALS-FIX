using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; // Needed for loading scenes

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3, 10)]
    public string[] sentences; 
    public float typingSpeed = 0.05f;

    public GameObject playButton; // Drag your Play Button here
    public string gameSceneName;  // Type your Level 1 scene name here

    private int index;
    private bool isTyping;
    private bool dialogueFinished;

    void OnEnable()
    {
        index = 0;
        dialogueFinished = false;
        playButton.SetActive(false); // Hide button at start
        StartCoroutine(Type());
    }

    void Update()
    {
        // Only allow clicking to next sentence if we aren't finished yet
        if (!dialogueFinished && (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textDisplay.text = sentences[index];
                isTyping = false;
            }
            else
            {
                NextSentence();
            }
        }
    }

    IEnumerator Type()
    {
        isTyping = true;
        textDisplay.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(Type());
        }
        else
        {
            // DIALOGUE ENDED
            dialogueFinished = true;
            playButton.SetActive(true); // Show the Play Button!
        }
    }

    // This function will be linked to the Play Button's OnClick
    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}