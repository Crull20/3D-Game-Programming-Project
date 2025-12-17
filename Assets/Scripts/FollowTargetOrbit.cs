using UnityEngine;

public class FollowTargetOrbit : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 localOffset = new Vector3(0.3f, 0.2f, 0.7f);

    public float yawSensitivity = 250f;
    public float pitchSensitivity = 180f;
    public float minPitch = -35f;
    public float maxPitch = 70f;

    public bool invertX;
    public bool invertY;

    float yaw;
    float pitch;

    void Awake()
    {
        // initialize from current rig rotation
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = NormalizeAngle(e.x);

        if (followTarget)
            followTarget.localPosition = localOffset;
    }

    void Update()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        if (invertX) mx = -mx;
        if (invertY) my = -my;

        yaw += mx * yawSensitivity * Time.deltaTime;
        pitch -= my * pitchSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // rotate the rig; the child offset rotates with it
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // enforce offset (in case something edits it)
        if (followTarget) followTarget.localPosition = localOffset;
    }

    static float NormalizeAngle(float a)
    {
        while (a > 180f) a -= 360f;
        while (a < -180f) a += 360f;
        return a;
    }
}
