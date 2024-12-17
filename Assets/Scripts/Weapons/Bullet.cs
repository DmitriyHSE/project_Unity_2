using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public Light bulletLight;


    private void Start()
    {
        Debug.Log("Bullet will be destroyed in " + lifetime + " seconds.");
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.collider.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}