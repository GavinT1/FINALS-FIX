using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonBlink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;
    public float blinkSpeed = 0.2f;
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        InvokeRepeating("ToggleText", 0, blinkSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        CancelInvoke("ToggleText");
        buttonText.enabled = true; // Ensure text stays visible when leaving
    }

    void ToggleText()
    {
        buttonText.enabled = !buttonText.enabled;
    }
}