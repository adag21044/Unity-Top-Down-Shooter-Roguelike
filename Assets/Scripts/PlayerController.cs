using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 50.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public PlayerInput playerInput;
    public EnemyDetector enemyDetector; // EnemyDetector script'inin referansı
    public GameObject muzzlePrefab; // Muzzle prefab'ı
    public Transform shootPoint; // Muzzle'ın çıkış noktası
    private float shootInterval = 1.0f; // Ateş etme aralığı
    private float lastShootTime; // Son ateş zamanı

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Null kontrolü yapın
        if (enemyDetector != null && enemyDetector.detectedEnemyTransform != null)
        {
            Vector3 directionToEnemy = enemyDetector.detectedEnemyTransform.position - transform.position;
            directionToEnemy.y = 0;

            if (directionToEnemy != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // Ateş etme zamanı geldi mi kontrol et
                if (Time.time > lastShootTime + shootInterval)
                {
                    Shoot(); // Shoot fonksiyonunda tekrar null kontrolü yapılacak
                    lastShootTime = Time.time;
                }
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    public void Shoot()
    {
        if (muzzlePrefab == null)
        {
            Debug.LogError("Muzzle prefab is null");
            return;
        }

        // Muzzle prefab'ını instantiate et
        GameObject muzzleInstance = Instantiate(muzzlePrefab, shootPoint.position, shootPoint.rotation);

        // MuzzleController'ı al
        MuzzleController muzzleController = muzzleInstance.GetComponent<MuzzleController>();

        if (enemyDetector.detectedEnemyTransform == null)
        {
            Debug.LogWarning("No detected enemy, destroying muzzle instance");
            Destroy(muzzleInstance);
            return;
        }

        if (muzzleController == null)
        {
            Debug.LogError("MuzzleController component is missing on the muzzle instance");
            Destroy(muzzleInstance);
            return;
        }

        Vector3 directionToEnemy = (enemyDetector.detectedEnemyTransform.position - shootPoint.position).normalized;
        muzzleController.SetDirection(directionToEnemy);
    }


}
