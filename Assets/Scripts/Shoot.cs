using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Gun gun;

    private void OnShoot()
    {
        gun.Shoot();
    }
    
}
