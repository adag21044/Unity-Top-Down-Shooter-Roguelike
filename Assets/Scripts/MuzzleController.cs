using UnityEngine;

public class MuzzleController : MonoBehaviour
{
    public float speed = 10f; // Muzzle'ın hızı
    public float destroyDelay = 0.1f; // Çarpmadan sonra yok edilmeden önceki gecikme süresi
    public float lifeSpan = 5f; // Muzzle'ın yaşam süresi
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = transform.forward; // Varsayılan olarak ileri doğru gider

        // Muzzle'ı yaşam süresi dolduktan sonra yok et
        //Destroy(gameObject, lifeSpan);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Update()
    {
        // Muzzle'ı belirlenen yönde hareket ettir
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.CompareTag("Enemy"))
        {
            // EnemyMove scriptini devre dışı bırak
            var enemyMove = other.GetComponent<EnemyMove>();
            if (enemyMove != null)
            {
                enemyMove.enabled = false; // Yok etmeden önce hareketi durdur
            }

            Debug.Log(other.name + " is hit by muzzle");

            Destroy(other.gameObject); // Hedefi yok et
            //Destroy(gameObject, destroyDelay); // Muzzle'ı yok et
        }
    }

}
