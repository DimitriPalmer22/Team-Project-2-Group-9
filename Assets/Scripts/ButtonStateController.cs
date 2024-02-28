using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStateController : MonoBehaviour
{
    public Image fillImage; 
    public bool isMasterButton;

    private void Awake()
    {
        if (ButtonStateManager.Instance != null)
        {
            ButtonStateManager.Instance.OnMasterButtonStateChanged += UpdateButtonAppearance;
            ButtonStateManager.Instance.OnNoviceButtonStateChanged += UpdateButtonAppearance;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        UpdateButtonAppearance();
    }

    private void OnEnable()
    {
        UpdateButtonAppearance();
    }

    private void OnDisable()
    {
        if (ButtonStateManager.Instance != null)
        {
            ButtonStateManager.Instance.OnMasterButtonStateChanged -= UpdateButtonAppearance;
            ButtonStateManager.Instance.OnNoviceButtonStateChanged -= UpdateButtonAppearance;
        }
    }

    private void OnButtonClick()
    {
        if (isMasterButton)
        {
            ButtonStateManager.Instance.ToggleMasterButtonState();
        }
        else
        {
            ButtonStateManager.Instance.ToggleNoviceButtonState();
        }
    }

    private void UpdateButtonAppearance()
    {
        if (isMasterButton)
        {
            fillImage.enabled = ButtonStateManager.Instance.IsMasterButtonFilled();
        }
        else
        {
            fillImage.enabled = ButtonStateManager.Instance.IsNoviceButtonFilled();
        }

    }
}

