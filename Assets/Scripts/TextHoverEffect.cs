using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textMeshProComponent;
    private Color originalColor;

    void Start()
    {
        textMeshProComponent = GetComponent<TMP_Text>();

        if (textMeshProComponent != null)
        {
            originalColor = textMeshProComponent.color;
        }
        else
        {
            Debug.LogError("TMP_Text component not found on GameObject: " + gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshProComponent != null)
        {
            textMeshProComponent.color = new Color32(242, 223, 119, 180);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshProComponent != null)
        {
            textMeshProComponent.color = originalColor;
        }
    }

    void OnEnable()
    {
        if (textMeshProComponent != null)
        {
            textMeshProComponent.color = originalColor;
        }
    }
}
