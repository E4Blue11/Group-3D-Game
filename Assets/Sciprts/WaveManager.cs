using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Zombies")]
    public GameObject zombiePrefab;

    public int startingZombies = 4;
    public int extraZombiesPerWave = 1;
    public float timeBetweenWaves = 3f;

    [Header("Zombie Difficulty")]
    public float baseZombieSpeed = 1.5f;
    public float speedIncreaseEveryFiveWaves = 0.25f;

    [Header("Arena")]
    public Vector2 arenaMin = new Vector2(-11.5f, -11.5f);
    public Vector2 arenaMax = new Vector2(11.5f, 11.5f);

    public float minimumDistanceFromPlayer = 3f;

    [Header("UI")]
    public TMP_Text waveText;

    private Transform player;
    private int currentWave = 1;
    private bool waitingForNextWave;
    private float waveTimer;

    public int CurrentWave => currentWave;

    void Start()
    {
        // Find the player
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        UpdateWaveUI();
        SpawnWave();
    }

    void Update()
    {
        // Check if all zombies are defeated
        if (!waitingForNextWave &&
            FindObjectsByType<ZombieHealth>().Length == 0)
        {
            waitingForNextWave = true;
            waveTimer = timeBetweenWaves;

            Debug.Log("Wave cleared!");
        }

        // Wait before starting the next wave
        if (waitingForNextWave)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0f)
            {
                currentWave++;
                waitingForNextWave = false;

                UpdateWaveUI();
                SpawnWave();
            }
        }
    }

    void SpawnWave()
    {
        // Calculate zombies for this wave
        int zombiesThisWave =
            startingZombies +
            (currentWave - 1) * extraZombiesPerWave;

        // Increase zombie speed every five waves
        int speedIncreaseCount = currentWave / 5;

        float currentZombieSpeed =
            baseZombieSpeed +
            speedIncreaseCount *
            speedIncreaseEveryFiveWaves;

        Debug.Log(
            "Starting Wave " +
            currentWave +
            " with " +
            zombiesThisWave +
            " zombies."
        );

        // Spawn zombies on the NavMesh
        for (int i = 0; i < zombiesThisWave; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            GameObject newZombie = Instantiate(
                zombiePrefab,
                spawnPosition,
                Quaternion.identity
            );

            NavMeshAgent agent =
                newZombie.GetComponent<NavMeshAgent>();

            if (agent != null)
                agent.speed = currentZombieSpeed;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Find a random walkable position
        for (int attempt = 0; attempt < 30; attempt++)
        {
            float randomX =
                Random.Range(arenaMin.x, arenaMax.x);

            float randomZ =
                Random.Range(arenaMin.y, arenaMax.y);

            Vector3 randomPosition =
                new Vector3(randomX, 0f, randomZ);

            if (player != null &&
                Vector3.Distance(
                    randomPosition,
                    player.position
                ) < minimumDistanceFromPlayer)
            {
                continue;
            }

            if (NavMesh.SamplePosition(
                randomPosition,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
            {
                return hit.position + Vector3.up;
            }
        }

        return new Vector3(5f, 1f, 5f);
    }

    void UpdateWaveUI()
    {
        // Display the current wave
        if (waveText != null)
            waveText.text = "Wave: " + currentWave;
    }
}
