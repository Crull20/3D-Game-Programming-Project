using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [Tooltip("If true, the sprite only rotates around Y, staying upright.")]
    public bool lockYOnly = true;

    private Transform cam;

    void LateUpdate()
    {
        // Cache camera reference
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }

        if (lockYOnly)
        {
            // Face camera but stay upright (no tilting on slopes)
            Vector3 dir = transform.position - cam.position;
            dir.y = 0f; // ignore vertical difference
            if (dir.sqrMagnitude < 0.0001f) return;

            transform.rotation = Quaternion.LookRotation(dir);

            // If your sprite faces -Z in its texture, flip 180 degrees:
            // transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            // Full look at camera (can tilt, usually not what you want)
            transform.LookAt(cam);

            // Rotate 180 if the sprite's "front" is backwards
            // transform.Rotate(0f, 180f, 0f);
        }
    }
}