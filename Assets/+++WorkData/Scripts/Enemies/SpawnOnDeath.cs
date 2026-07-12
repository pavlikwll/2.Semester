//Volodymyr Pavlik
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;

    [Min(1)]
    [SerializeField] private int spawnAmount = 3;

    [Min(0f)]
    [SerializeField] private float spawnRadius = 0.7f;

    public void SpawnAt(Vector3 centerPosition)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError(
                $"Prefab To Spawn is missing on {gameObject.name}.",
                this
            );
            return;
        }
        
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = centerPosition + (Vector3)randomOffset;
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}