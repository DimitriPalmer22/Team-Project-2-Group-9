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
        UpdateButtonAppearance();
    }

    private void OnEnable()
    {
        UpdateButtonAppearance();
    }

    private void OnButtonClick()
    {
        if (isMasterButton)
        {
            // Toggle the master button state
            ButtonStateManager.ToggleMasterButtonState();
            
            // If the novice button is filled, force the novice button to be unfilled
            if (ButtonStateManager.IsNoviceButtonFilled)
                ButtonStateManager.ToggleNoviceButtonState();

        }

        else
        {
            // Toggle the novice button state
            ButtonStateManager.ToggleNoviceButtonState();

            // If the master button is filled, force the master button to be unfilled
            if (ButtonStateManager.IsMasterButtonFilled)
                ButtonStateManager.ToggleMasterButtonState();
        }

        // Update the button appearance
        UpdateButtonAppearance();
    }

    private void UpdateButtonAppearance()
    {
        fillImage.enabled = (isMasterButton) 
            ? ButtonStateManager.IsMasterButtonFilled 
            : ButtonStateManager.IsNoviceButtonFilled;
    }
}