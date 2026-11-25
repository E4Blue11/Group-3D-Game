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
    private Sandevistan sandevistan;
    private float attackTimer;

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
            sandevistan = playerObject.GetComponent<Sandevistan>();
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

        // Slow attack timer during Sandevistan
        float attackSpeed = 1f;

        if (sandevistan != null && sandevistan.IsActive)
            attackSpeed = sandevistan.slowMultiplier;

        attackTimer -= Time.deltaTime * attackSpeed;

        // Attack when close enough
        if (distance <= attackRange && attackTimer <= 0f)
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
        attackTimer = attackCooldown;
    }
}
