using UnityEngine;
using TMPro;
using System.Collections;

public class HACKERTEXT : MonoBehaviour
{
    public TMP_Text textComponent;
    public float changeSpeed = 0.05f; // How fast characters swap
    private string originalText;
    private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&";

    void Start()
    {
        originalText = textComponent.text;
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            char[] tempText = originalText.ToCharArray();
            for (int i = 0; i < tempText.Length; i++)
            {
                
                if (tempText[i] != ' ' && tempText[i] != '\n')
                {
                    
                    if (Random.value > 0.8f) 
                        tempText[i] = chars[Random.Range(0, chars.Length)];
                }
            }
            textComponent.text = new string(tempText);
            yield return new WaitForSeconds(changeSpeed);
        }
    }
}