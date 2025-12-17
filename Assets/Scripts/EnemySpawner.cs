using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;                // the thing that actually moves
    public float spawnInterval = 2f;
    public int maxAlive = 10;

    public float spawnRadius = 12f;         // distance from player
    public float minSpawnDistance = 6f;     // don’t spawn on top of player

    float timer;

    void Update()
    {
        if (enemyPrefab == null || player == null) return;

        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxAlive) return;

        Vector3 spawnPos = PickSpawnPos();
        GameObject e = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // tell the enemy who to chase
        var ai = e.GetComponent<EnemyAI>();
        if (ai != null) ai.target = player;
    }

    Vector3 PickSpawnPos()
    {
        // pick a point on a ring around the player
        Vector2 r = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, spawnRadius);
        Vector3 pos = player.position + new Vector3(r.x, 0f, r.y);

        // optional: raycast to ground if you have terrain
        if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 200f))
            pos.y = hit.point.y;

        return pos;
    }
}
