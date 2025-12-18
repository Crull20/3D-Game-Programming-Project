using UnityEditor;
using UnityEngine;

public class BillboardTerrainPainter : EditorWindow
{
    [Header("Prefab (your billboard bush)")]
    public GameObject prefab;

    [Header("Placement")]
    public float radius = 2.0f;
    public int countPerClick = 8;
    public LayerMask terrainMask = ~0; // set to Terrain layer if you have one

    [Header("Randomization")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public bool randomYaw = true;

    [MenuItem("Tools/Billboard Terrain Painter")]
    static void Open() => GetWindow<BillboardTerrainPainter>("Billboard Painter");

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        radius = EditorGUILayout.FloatField("Radius", radius);
        countPerClick = EditorGUILayout.IntField("Count / Click", countPerClick);
        terrainMask = LayerMaskField("Terrain Mask", terrainMask);

        scaleRange = EditorGUILayout.Vector2Field("Scale Range", scaleRange);
        randomYaw = EditorGUILayout.Toggle("Random Yaw", randomYaw);

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Scene View: Shift + Left Click to paint.", MessageType.Info);
    }

    void OnEnable() => SceneView.duringSceneGui += DuringSceneGUI;
    void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    void DuringSceneGUI(SceneView view)
    {
        Event e = Event.current;
        if (e == null) return;

        // Shift + Left click
        if (e.type == EventType.MouseDown && e.button == 0 && e.shift)
        {
            if (prefab == null)
            {
                Debug.LogWarning("Assign a prefab in Tools > Billboard Terrain Painter.");
                return;
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10000f, terrainMask, QueryTriggerInteraction.Ignore))
            {
                PaintAt(hit.point, hit.normal);
                e.Use(); // consume event so Unity doesn't also select stuff
            }
        }
    }

    void PaintAt(Vector3 center, Vector3 normal)
    {
        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();

        for (int i = 0; i < Mathf.Max(1, countPerClick); i++)
        {
            Vector2 r = Random.insideUnitCircle * radius;
            Vector3 p = center + new Vector3(r.x, 0f, r.y);

            // project down onto terrain
            if (Physics.Raycast(p + Vector3.up * 500f, Vector3.down, out RaycastHit hit, 2000f, terrainMask, QueryTriggerInteraction.Ignore))
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                Undo.RegisterCreatedObjectUndo(go, "Paint Billboard");

                go.transform.position = hit.point;

                // keep upright; your BillboardSprite rotates later anyway :contentReference[oaicite:2]{index=2}
                go.transform.rotation = Quaternion.identity;
                if (randomYaw) go.transform.Rotate(0f, Random.Range(0f, 360f), 0f);

                float s = Random.Range(scaleRange.x, scaleRange.y);
                go.transform.localScale = Vector3.one * s;
            }
        }

        Undo.CollapseUndoOperations(group);
    }

    // Helper for LayerMask in EditorWindow
    static LayerMask LayerMaskField(string label, LayerMask selected)
    {
        var layers = UnityEditorInternal.InternalEditorUtility.layers;
        int mask = selected.value;
        mask = EditorGUILayout.MaskField(label, mask, layers);
        selected.value = mask;
        return selected;
    }
}