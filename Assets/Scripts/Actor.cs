using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// The maximum health of the actor
    /// </summary>
    [SerializeField] private int _maxHealth;
    
    /// <summary>
    /// The current health of the actor
    /// </summary>
    [SerializeField] private int _currentHealth;

    #endregion

    #region Properties

    // The current health of the actor
    public int CurrentHealth => _currentHealth;

    // The maximum health of the actor
    public int MaxHealth => _maxHealth;
    
    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the max health is at least 1
        if (_maxHealth <= 0)
            _maxHealth = 1;
        
        // Set the current health to the max health
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion
    
    #region Methods

    public virtual void ChangeHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

        // Lose health
        if (amount < 0)
        {
            Debug.Log($"Lost {amount} health. Current health: {_currentHealth}");
        }
        // Gain health
        else
        {
            Debug.Log($"Gained {amount} health. Current health: {_currentHealth}");            
        }
        
        if (_currentHealth <= 0)
            Die();
    }

    protected abstract void Die();

    #endregion


}
