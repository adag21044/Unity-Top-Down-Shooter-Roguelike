using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab; // Collectible prefab'ı
    [SerializeField] private float collectibleSpawnOffset = 1f; // Collectible'ın spawn edileceği offset

    private void OnDestroy()
    {
        // Collectible'ı instantiate et
        if (collectiblePrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * collectibleSpawnOffset;
            Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
