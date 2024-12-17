using System.Collections;
using UnityEngine;

public class Pistol : Gun
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public ParticleSystem muzzleFlash;
    public Animator animator;
    public AudioSource pistolShot;
    public string shootAnimationName = "ShotAnimation";
    public float bulletSpeed = 300f;
    public float lastShootTime = 0f;


    public override void Shoot()
    {
        if (lastShootTime + 1/fireRate < Time.time)
        {
            lastShootTime = Time.time;

            pistolShot.Play();
            muzzleFlash.Play();
            animator.Play(shootAnimationName);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
            }
        }
    }
}
