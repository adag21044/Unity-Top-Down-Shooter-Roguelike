using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 50f; // Tüm düşmanların tespit edileceği maksimum mesafe
    public LayerMask detectionLayer;
    public Transform detectedEnemyTransform { get; private set; } // Tespit edilen düşmanın pozisyonu
    public float detectionThreshold = 50f; // Düşman tespiti için mesafe eşiği

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        Transform closestEnemy = null;
        float closestDistance = detectionThreshold; // Mesafe eşiği ile başlat

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log(hitCollider.name + " is detected");
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        detectedEnemyTransform = closestEnemy; // En yakın düşmanın pozisyonu

        if (closestEnemy != null)
        {
            Debug.Log("Closest enemy is " + closestEnemy.name);
        }
    }
}
