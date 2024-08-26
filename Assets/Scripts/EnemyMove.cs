using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private float moveSpeed = 10f;
    private float rotationSpeed = 3f;

    private void Update()
    {
        // Eğer playerTransform veya gameObject yok edilmişse, hareket etmeyi durdur
        if (playerTransform == null || this == null) return;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
