using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineVirtualCamera vcam;
    public Transform lookAtTarget;

    [Header("Mouse Look")]
    public float yawSensitivity = 250f;
    public float pitchSensitivity = 180f;
    public bool invertX;
    public bool invertY;
    public float minPitch = -35f;
    public float maxPitch = 70f;

    [Header("Cursor")]
    public bool lockCursor = true;

    float yaw;
    float pitch;

    void Awake()
    {
        if (!vcam) vcam = FindObjectOfType<CinemachineVirtualCamera>();

        if (vcam)
        {
            vcam.Follow = transform;
            if (lookAtTarget) vcam.LookAt = lookAtTarget;
        }

        // Start from current pivot rotation, but keep it sane
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = NormalizeAngle(e.x);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void Start()
    {
        SetCursor(lockCursor);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (lockCursor) SetCursor(hasFocus);
    }

    void Update()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        if (invertX) mx = -mx;
        if (invertY) my = -my;

        yaw += mx * yawSensitivity * Time.deltaTime;
        pitch -= my * pitchSensitivity * Time.deltaTime; // minus makes "mouse up = look up"
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void SetCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    static float NormalizeAngle(float a)
    {
        while (a > 180f) a -= 360f;
        while (a < -180f) a += 360f;
        return a;
    }
}
