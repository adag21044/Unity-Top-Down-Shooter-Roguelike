using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 50.0f;
    private float gravityValue = -9.81f;
    public PlayerInput playerInput;
    public EnemyDetector enemyDetector; // EnemyDetector script'inin referansı
    public GameObject muzzlePrefab; // Muzzle prefab'ı
    public Transform shootPoint; // Muzzle'ın çıkış noktası
    public ObjectPool muzzlePool;

    private int maxBullets = 5; // Maksimum mermi sayısı
    private int currentBullets; // Mevcut mermi sayısı
    private float reloadTime = 1f; // Mermi dolum süresi
    private float shootInterval = 0.5f; // Ateş etme aralığı
    private float lastShootTime; // Son ateş zamanı
    private float lastReloadTime; // Son yeniden yükleme zamanı

    [SerializeField] private List<Image> bulletIcons = new List<Image>(); // UI Mermi ikonları

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
        currentBullets = maxBullets;
        UpdateUI();
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

        // Mermi yeniden yükleme kontrolü
        if (Time.time > lastReloadTime + reloadTime && currentBullets < maxBullets)
        {
            currentBullets++;
            UpdateUI();
            lastReloadTime = Time.time;
        }

        // Düşman tespiti ve ateş etme kontrolü
        if (enemyDetector != null && enemyDetector.detectedEnemyTransform != null)
        {
            Vector3 directionToEnemy = enemyDetector.detectedEnemyTransform.position - transform.position;
            directionToEnemy.y = 0;

            if (directionToEnemy != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // Ateş etme zamanı geldi mi kontrol et ve mermiler varsa ateş et
                if (currentBullets > 0 && Time.time > lastShootTime + shootInterval)
                {
                    Shoot();
                    currentBullets--;
                    UpdateUI();
                    lastShootTime = Time.time;
                }
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < bulletIcons.Count; i++)
        {
            if (i < currentBullets)
            {
                bulletIcons[i].color = new Color(1, 1, 1, 1); // Normal görünüm
            }
            else
            {
                // Transparan görünüm
                // Mevcut renk değerini al
                Color color = bulletIcons[i].color;

                // Alpha değerini güncelle
                color.a = 0.33f;

                // Güncellenmiş rengi Image bileşenine ata
                bulletIcons[i].color = color;
            }
        }
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    public void Shoot()
    {
        GameObject muzzleInstance = muzzlePool.GetObject();
        muzzleInstance.transform.position = shootPoint.position;
        muzzleInstance.transform.rotation = shootPoint.rotation;

        MuzzleController muzzleController = muzzleInstance.GetComponent<MuzzleController>();
        Vector3 directionToEnemy = (enemyDetector.detectedEnemyTransform.position - shootPoint.position).normalized;
        muzzleController.SetDirection(directionToEnemy);
    }
}
