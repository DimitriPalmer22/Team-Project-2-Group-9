using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonStateController : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private bool isMasterButton;

    private void Awake()
    {
        UpdateButtonAppearance();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        UpdateButtonAppearance();
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        UpdateButtonAppearance();
    }

    private void OnButtonClick()
    {
        if (isMasterButton)
            ButtonStateManager.ToggleMasterButtonState();

        else
            ButtonStateManager.ToggleNoviceButtonState();

        // Update the button appearance
        UpdateButtonAppearance();
    }

    private void UpdateButtonAppearance()
    {
        if (isMasterButton)
            fillImage.enabled = ButtonStateManager.IsMasterButtonFilled;

        else
            fillImage.enabled = ButtonStateManager.IsNoviceButtonFilled;
    }
}