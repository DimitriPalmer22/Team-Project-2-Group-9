using TMPro;
using UnityEngine;

public class InGameUIManagerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _spellInfoText;
    [SerializeField] private TMP_Text _spellDurationText;
    [SerializeField] private TMP_Text _pickupText;
    [SerializeField] private TMP_Text _masterNoviceText;

    // The camera
    private Camera _camera;

    // The spell cast script
    private SpellCastScript _spellCastScript;

    // The actor player script
    private ActorPlayer _actorPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the camera
        _camera = Camera.main;

        // Get the spell cast script
        _spellCastScript = GetComponent<SpellCastScript>();

        // Get the actor player script
        _actorPlayer = GetComponent<ActorPlayer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the health text
        UpdateHealthText();

        // Update the spell info text
        UpdateSpellInfoText();

        // Update the spell duration text
        UpdateSpellDurationText();

        // Determine what the player is looking at & update the pickup text
        DetermineLookingAt();

        // Update the master/novice text
        UpdateMasterNoviceText();
    }

    private void UpdateHealthText()
    {
        // Get the player's health
        var playerHealth = _actorPlayer.CurrentHealth;

        // Set the health text
        _healthText.text = $"Health: {playerHealth}";
    }

    private void UpdateSpellInfoText()
    {
        // Get the current spell
        var currentSpell = _spellCastScript.SpellType;

        // Set the spell info text
        _spellInfoText.text = $"Current Spell: {currentSpell}\n";

        // if the current spell is none, return
        if (currentSpell == SpellCastType.None)
            return;

        // Add the spell's remaining uses
        _spellInfoText.text += $"Remaining Uses: {_spellCastScript.RemainingUses}";
    }

    private void UpdateSpellDurationText()
    {
        // If the current spell is not a duration spell, return
        // If the current spell is not active, return
        if (!_spellCastScript.SpellType.IsDurationSpellType() || !_spellCastScript.IsSpellActive)
        {
            _spellDurationText.text = "";
            return;
        }

        // Set the spell duration text to the remaining duration of the spell
        _spellDurationText.text = $"{_spellCastScript.SpellType}'s Remaining Duration:\n" +
                                  $"{_spellCastScript.RemainingDuration:0.00} Seconds";
    }

    private void DetermineLookingAt()
    {
        // Get the camera transform
        var cameraTransform = _camera.transform;

        // Ray cast from the camera
        var hitAGameObject = Physics.Raycast(
            cameraTransform.position,
            cameraTransform.forward, out var hit, SpellCastScript.PickupDistance);

        // If the player is not looking at a GameObject, return
        if (!hitAGameObject)
        {
            // Hide the pickup text
            _pickupText.text = "";

            return;
        }

        // Determine what the player is looking at
        switch (hit.transform.tag)
        {
            case "Spell Pickup":
                // Get the spell's script
                var spellScript = hit.transform.GetComponent<SpellPickupScript>();

                // Set the pickup text
                _pickupText.text = $"Press E to pick up {spellScript.SpellType} spell.";
                break;

            case "Win Object":
                // Set the pickup text
                _pickupText.text = $"Press E to pick up the Staff!";
                break;
        }
    }

    private void UpdateMasterNoviceText()
    {
        if (ButtonStateManager.IsMasterButtonFilled)
            _masterNoviceText.text = "Master Mode Enabled";

        else if (ButtonStateManager.IsNoviceButtonFilled)
            _masterNoviceText.text = "Novice Mode Enabled";

        else
            _masterNoviceText.text = "";
    }
}