using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TEXTGLOWHOVER : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMesh;
    private Material textMaterial;

    [Header("Border Settings")]
    public Image sideBorder;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        
        textMaterial = textMesh.fontSharedMaterial; 
        textMaterial.EnableKeyword("GLOW_ON");
        textMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0); 

        if(sideBorder != null) sideBorder.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 1.0f); 
        textMesh.color = new Color(0.96f, 0.75f, 0.45f); 

        if(sideBorder != null)
        {
            sideBorder.gameObject.SetActive(true);
            sideBorder.color = new Color(0.96f, 0.75f, 0.45f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f); 
        textMesh.color = new Color(0.95f, 0.92f, 0.81f);

        if(sideBorder != null) sideBorder.gameObject.SetActive(false);
    }
}