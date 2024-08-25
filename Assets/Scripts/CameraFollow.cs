using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (PlayerController.Instance != null)
        {
            Transform playerTransform = PlayerController.Instance.GetPlayerTransform();
            mainCamera.transform.position = playerTransform.position + new Vector3(0f, 54f, -50f);
        }
    }
}
