using UnityEngine;
using KH;


#if UNITY_EDITOR
using UnityEditor;

// This tells Unity: “This custom editor edits SpawnEvent objects.”
[CustomEditor(typeof(SpawnEvent))]
public class SpawnEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the normal fields first (enemyID, spawnPosition, spawnTime)
        base.OnInspectorGUI();

        // Get a reference to the target SpawnEvent we're editing
        SpawnEvent spawnEvent = (SpawnEvent)target;

        EditorGUILayout.Space();

        // Add a button to the inspector
        if (GUILayout.Button("Set Position From Selected Object"))
        {
            if (Selection.activeTransform != null)
            {
                // Copy the selected object's position into the ScriptableObject
                spawnEvent.spawnPoint = Selection.activeTransform.position;

                // Mark the object as "dirty" so Unity saves the change
                EditorUtility.SetDirty(spawnEvent);

                Debug.Log($"Spawn position set to {spawnEvent.spawnPoint} from {Selection.activeTransform.name}");
            }
            else
            {
                Debug.LogWarning("No object selected in the Hierarchy!");
            }
        }
    }
}
#endif
