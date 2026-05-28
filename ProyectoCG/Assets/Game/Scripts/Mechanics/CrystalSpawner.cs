using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    public GameObject crystalPrefab;

    public int amountToSpawn = 5;

    public Vector3 spawnAreaSize;

    void Start()
    {
        SpawnCrystals();
    }

    void SpawnCrystals()
    {
        for(int i = 0; i < amountToSpawn; i++)
        {
            Vector3 randomPosition = transform.position +
            new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            Instantiate(crystalPrefab, randomPosition, Quaternion.identity);
        }
    }
}