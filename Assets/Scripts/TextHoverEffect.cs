using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text _textMeshProComponent;
    private Color _originalColor;

    private void Start()
    {
        _textMeshProComponent = GetComponent<TMP_Text>();

        if (_textMeshProComponent != null)
            _originalColor = _textMeshProComponent.color;
        
        else
            Debug.LogError("TMP_Text component not found on GameObject: " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_textMeshProComponent != null)
            _textMeshProComponent.color = new Color32(242, 223, 119, 180);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_textMeshProComponent != null)
            _textMeshProComponent.color = _originalColor;
    }

    private void OnEnable()
    {
        if (_textMeshProComponent != null)
            _textMeshProComponent.color = _originalColor;
    }
}
