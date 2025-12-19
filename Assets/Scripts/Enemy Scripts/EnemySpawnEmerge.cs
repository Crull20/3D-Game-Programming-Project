using UnityEngine;

public class EnemySpawnEmerge : MonoBehaviour
{
    public float emergeHeight = 1.5f;   // how far up it rises
    public float emergeTime = 1.2f;     // how long it takes

    EnemyAI ai;
    Rigidbody rb;
    Vector3 startPos;
    Vector3 endPos;
    float t;

    void Awake()
    {
        ai = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody>();

        if (ai != null) ai.enabled = false; // stop chasing during spawn
        if (rb != null) rb.isKinematic = true; // freeze physics during rise
    }

    void Start()
    {
        startPos = transform.position - Vector3.up * emergeHeight;
        endPos = transform.position;
        transform.position = startPos;
    }

    void Update()
    {
        t += Time.deltaTime / emergeTime;
        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (t >= 1f)
        {
            if (rb != null) rb.isKinematic = false;
            if (ai != null) ai.enabled = true;

            Destroy(this); // remove script after done
        }
    }
}
