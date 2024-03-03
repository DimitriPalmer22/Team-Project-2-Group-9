using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DetectionBarScript : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private Image _barForeground;
    [SerializeField] private Image _barBackround;
    [SerializeField] private TMP_Text _barText;

    [SerializeField] private Slider _slider;
    
    [SerializeField] private EnemyController _enemyController;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Get the camera
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        // Set the colors of the bar
        SetColors();
        
        // Set the transparency of the bar
        SetBarTransparency();
        
        // Set the fill of the bar
        SetBarFill();

        // Set the text of the bar
        SetBarText();
    }

    private void LateUpdate()
    {
        // Look at the camera
        transform.LookAt(_camera.transform);
    }

    private float GetTransparency()
    {
        // If the enemy is frozen, the bar is visible
        if (_enemyController.IsFrozen)
            return 1;

        // The cutoff for the enemy's spotting percentage.
        // Treat this as 100% when determining the transparency of the bar.
        const float cutoff = 0.5f;
        
        // If the enemy's spotting percentage is greater than or equal to the cutoff, the bar is fully visible
        float transparency = Mathf.Clamp(_enemyController.SpottingProgress / cutoff, 0, 1);
        
        return transparency;
    }

    private void SetBarTransparency()
    {
        // Get the transparency of the bar
        float transparency = GetTransparency();
        
        // Set the transparency of the bar's foreground
        _barForeground.color = new Color(_barForeground.color.r, _barForeground.color.g, _barForeground.color.b, transparency);
        
        // Set the transparency of the bar's background
        _barBackround.color = new Color(_barBackround.color.r, _barBackround.color.g, _barBackround.color.b, transparency);
        
        // Set the transparency of the bar's text
        _barText.color = new Color(_barText.color.r, _barText.color.g, _barText.color.b, transparency);
    }

    private void SetBarFill()
    {
        // If the enemy is frozen, the bar is full
        if (_enemyController.IsFrozen)
        {
            _slider.value = 1;
            return;
        }
        
        // Set the fill of the slider
        _slider.value = _enemyController.SpottingProgress;
    }

    private void SetColors()
    {
        switch (_enemyController.PatrolState)
        {
            case EnemyPatrolState.Investigate:
                _barForeground.color = Color.yellow;
                _barText.color = Color.black;
                break;
            
            case EnemyPatrolState.Chase:
                _barForeground.color = Color.red;
                _barText.color = Color.black;
                break;
            
            case EnemyPatrolState.Lost:
                _barForeground.color = new Color(1, .5f, 0);
                _barText.color = Color.black;
                break;
        }
        
        if (_enemyController.IsFrozen)
            _barForeground.color = Color.cyan;
    }

    private void SetBarText()
    {
        switch (_enemyController.PatrolState)
        {
            case EnemyPatrolState.Investigate:
                _barText.text = "Investigating";
                break;
            
            case EnemyPatrolState.Chase:
                _barText.text = "Chasing!";
                break;
            
            case EnemyPatrolState.Lost:
                _barText.text = "Losing Sight";
                break;
        }

        if (_enemyController.IsFrozen)
            _barText.text = "Frozen!";
    }
    
    
}
