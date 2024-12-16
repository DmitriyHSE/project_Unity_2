using UnityEngine;
using UnityEngine.AI;

public class HidingRobotAI : MonoBehaviour
{
    public float health = 100f;
    public float detectionRange = 20f;
    public float closeRange = 5f;
    public float fireRate = 1f;
    public float attackDamage = 15f;
    public Transform[] coverPoints;

    private GameObject player;
    private float lastFireTime;
    private NavMeshAgent navMeshAgent;
    private bool isRetreating;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = true;
        navMeshAgent.updateUpAxis = true;
        isRetreating = false;
    }

    private void Update()
    {
        if (isRetreating && navMeshAgent.remainingDistance < 0.5f)
        {
            isRetreating = false; // Stopped retreating after reaching cover
        }

        if (IsPlayerInRange(detectionRange) && !isRetreating)
        {
            RotateTowardsPlayer();

            if (IsPlayerInRange(closeRange))
            {
                RetreatFromPlayer();
            }
            else
            {
                TryShootPlayer();
            }
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private void TryShootPlayer()
    {
        if (Time.time - lastFireTime >= 1 / fireRate)
        {
            lastFireTime = Time.time;
            RaycastHit hit;
            Vector3 direction = (player.transform.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Hiding Robot hits the player!");
                    player.GetComponent<PlayerHealth>().Hurt(attackDamage);
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void RetreatFromPlayer()
    {
        Transform bestCover = FindBestCoverPoint();
        if (bestCover != null)
        {
            navMeshAgent.SetDestination(bestCover.position);
            isRetreating = true;
        }
    }

    private Transform FindBestCoverPoint()
    {
        Transform bestPoint = null;
        float maxDistance = 0f;

        foreach (Transform coverPoint in coverPoints)
        {
            float distanceToPlayer = Vector3.Distance(coverPoint.position, player.transform.position);
            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                bestPoint = coverPoint;
            }
        }

        return bestPoint;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

