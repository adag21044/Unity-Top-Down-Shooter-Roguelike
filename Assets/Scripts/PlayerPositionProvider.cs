using UnityEngine;

public class PlayerPositionProvider : MonoBehaviour, IPositionProvider
{
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
    
