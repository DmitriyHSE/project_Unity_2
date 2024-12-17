using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;
    public Camera fpsCamera;

    public virtual void Shoot() {}
}
