using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public PlayerHealth player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Target health: " + health);
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        player.TargetEliminated();
    }
}
