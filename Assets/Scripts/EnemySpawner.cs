using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyController[] enemyPrefabs;
    [SerializeField]
    private CinemachineCamera camera;
    [SerializeField]
    private Vector2[] spawnPositions;
    [SerializeField]
    private int enemiesInWave;
    [SerializeField]
    private float timeBetweenSpawns;
    private WaitForSeconds spawnWaitTime;
    [SerializeField]
    private bool firstWave;
    [SerializeField]
    private EnemySpawner nextWave;

    private float startTime;

    private void Start()
    {
        spawnWaitTime = new WaitForSeconds(timeBetweenSpawns);
        if (firstWave)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        StartCoroutine(RunWave());
    }

    private IEnumerator RunWave()
    {
        for (int i = 0; i < enemiesInWave; i++)
        {
            EnemyController enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector2 spawnPosition = FindPositionOutsideCamera();
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
            Instantiate(enemyPrefab, spawnPosition, randomRotation);
            yield return spawnWaitTime;
        }
        nextWave?.StartWave();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 position in spawnPositions)
        {
            Gizmos.DrawWireSphere(position, 0.5f);
        }
    }

    private Vector2 FindPositionOutsideCamera()
    {
        float width = camera.Lens.OrthographicSize;
        float height = width * camera.Lens.Aspect;
        Bounds onScreen = new Bounds(camera.transform.position, new Vector3(width, height, 0));
        Vector2[] positionsOutsideCamera = spawnPositions.Where(p => !onScreen.Contains(p)).ToArray();
        return positionsOutsideCamera[Random.Range(0, positionsOutsideCamera.Length)];
    }
}