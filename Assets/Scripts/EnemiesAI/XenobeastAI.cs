using UnityEngine;
using UnityEngine.AI;

public class XenobeastAI : MonoBehaviour
{
    public float detectionRange = 15f;
    public float visionAngle = 45f;
    public float moveSpeed = 5f;
    public float fireRate = 1f;
    public float attackDamage = 10f;
    public Transform[] patrolPoints;

    private GameObject player;
    private float lastFireTime;
    private NavMeshAgent navMeshAgent;
    private int currentPatrolIndex;
    private bool isChasing;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.updateRotation = true;
        navMeshAgent.updateUpAxis = true;
        currentPatrolIndex = 0;
        isChasing = false;
        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (IsPlayerVisible() || IsPlayerInDetectionRange())
        {
            RotateTowardsPlayer();
            isChasing = true;
            ChasePlayer();
            TryShootPlayer();
        }
        else
        {
            if (isChasing)
            {
                // Player lost, return to patrol after a delay
                isChasing = false;
                GoToNextPatrolPoint();
            }
            Patrol();
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < visionAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= detectionRange;
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
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
                    Debug.Log("Xenobeast hits the player!");
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

    private void Patrol()
    {
        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 1f)
        {
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
}


