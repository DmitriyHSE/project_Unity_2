using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void Hurt(float damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("You died");
    }
}
