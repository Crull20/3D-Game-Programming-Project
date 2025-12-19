using UnityEngine;

public class EnemySpawnEmergeVisual : MonoBehaviour
{
    public Transform visualRoot;          // drag EnemyObject here
    public float emergeHeight = 1.5f;
    public float emergeTime = 1.2f;

    EnemyAI ai;
    float t;
    Vector3 startLocal;
    Vector3 endLocal;

    void Awake()
    {
        ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;
    }

    void Start()
    {
        if (visualRoot == null) visualRoot = transform;

        endLocal = visualRoot.localPosition;
        startLocal = endLocal - Vector3.up * emergeHeight;

        visualRoot.localPosition = startLocal;
    }

    void Update()
    {
        if (visualRoot == null) return;

        t += Time.deltaTime / Mathf.Max(0.0001f, emergeTime);
        float u = Mathf.Clamp01(t);

        // smooth ease-out (no “linear” robotic feel)
        u = 1f - Mathf.Pow(1f - u, 3f);

        visualRoot.localPosition = Vector3.Lerp(startLocal, endLocal, u);

        if (u >= 1f)
        {
            if (ai != null) ai.enabled = true;
            Destroy(this);
        }
    }
}
