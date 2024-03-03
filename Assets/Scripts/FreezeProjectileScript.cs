using UnityEngine;

public class FreezeProjectileScript : MonoBehaviour
{

    // The number of rotations per second
    private const float RotationsPerSecond = 1;

    // The maximum lifetime of the projectile
    private const float MaxLifetime = 3f;
    
    // The speed of the projectile
    [SerializeField] private float Speed;
    
    // The direction the projectile is moving
    private Vector3 _direction;
    
    // The spell cast script that fired this projectile
    private SpellCastScript _spellCastScript;
    
    // The lifetime of the projectile
    private float _lifetime;
    
    // Update is called once per frame
    private void Update()
    {
        // Update the position of the projectile
        UpdatePosition();
        
        // Update the lifetime of the projectile
        _lifetime += Time.deltaTime;
        
        // If the lifetime is greater than the max lifetime, destroy the projectile
        if (_lifetime > MaxLifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is an enemy
        switch (other.gameObject.tag)
        {
            // If the other object is the player, do nothing
            case "Player":
                return;
            
            case "Enemy":
                // Get the enemy controller
                EnemyController enemyController = other.GetComponent<EnemyController>();

                // Freeze the enemy 
                _spellCastScript.StartCoroutine(_spellCastScript.FreezeEnemy(enemyController));

                Debug.Log($"ENEMY IS FROZEN");
                break;
            
                        
            // If the other object is a projectile, do nothing
            case "Projectile":
                return;
        }
        
        Debug.Log($"FREEZE SPELL HIT: {other.gameObject.tag}");
        
        // Destroy the game object
        Destroy(gameObject);
    }
    
    public void Fire(SpellCastScript spellCastScript, Vector3 direction)
    {
        // set the spell cast script
        _spellCastScript = spellCastScript;
        
        // set the direction
        _direction = direction.normalized;
    }

    private void UpdatePosition()
    {
        // move the projectile
        transform.position += _direction * (Speed * Time.deltaTime);
        
        // Rotate the projectile to make it look like it's spinning
        transform.Rotate(Vector3.one, RotationsPerSecond * 360 * Time.deltaTime);
    }


}
