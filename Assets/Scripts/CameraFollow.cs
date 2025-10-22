using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // El jugador
    public float smoothSpeed = 0.125f;
    public float fixedY = 7f;      // Posició Y fixa
    public float fixedZ = -10f;    // Posició Z fixa (perquè la càmera vegi el joc)

    void LateUpdate()
    {
        if (target == null) return;

        // Només seguiment en X
        float desiredX = target.position.x;
        Vector3 desiredPosition = new Vector3(desiredX, fixedY, fixedZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
