using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [Header("Movement")]
    public float detectionRange = 20f;
    public float attackRange = 1.5f;

    [Header("Attack")]
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private Transform player;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find the player
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // Chase the player
        if (distance <= detectionRange &&
            distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }

        // Attack when close enough
        if (distance <= attackRange &&
            Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (playerHealth == null)
            return;

        // Damage the player
        playerHealth.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackCooldown;
    }
}
