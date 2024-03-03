using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    
    // The number of rotations per second
    private const float RotationsPerSecond = 1;

    // The maximum lifetime of the projectile
    private const float MaxLifetime = 3f;

    // The speed multiplier for the projectile during master mode
    private const float MasterSpeedMultiplier = 1.25f;
    
    // The speed of the projectile
    [SerializeField] private float Speed;

    
    // The direction the projectile is moving
    private Vector3 _direction;
    
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
            // If the other object is an enemy, do nothing
            case "Enemy":
                return;
            
            // If the other object is the player, decrement the player's health            
            case "Player":

                // Get the player controller
                ActorPlayer playerController = other.GetComponent<ActorPlayer>();
                
                // Decrement the player's health
                playerController.ChangeHealth(-1);

                // Log the player's health
                Debug.Log($"Player is hit! Player's health: {playerController.CurrentHealth}");
                break;
            
            // If the other object is a projectile, do nothing
            case "Projectile":
                return;
        }

        Debug.Log($"Enemy Projectile Hit: {other.gameObject.tag}");

        // Destroy the game object
        Destroy(gameObject);
    }

    public void Fire(Vector3 direction)
    {
        // set the direction
        _direction = direction.normalized;
    }

    private void UpdatePosition()
    {
        float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled 
            ? MasterSpeedMultiplier 
            : 1;
        
        // move the projectile
        transform.position += _direction * (Speed * difficultyMultiplier * Time.deltaTime);
        
        // Rotate the projectile to make it look like it's spinning
        transform.Rotate(Vector3.one, RotationsPerSecond * 360 * Time.deltaTime);
    }
    
}
