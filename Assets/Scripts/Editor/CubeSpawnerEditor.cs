using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CubeSpawnerEditor : EditorWindow
{
    private Vector3 lastSpawnPosition;
    private List<GameObject> spawnedCubes;

    [MenuItem("Window/Cube Spawner")]
    public static void ShowWindow()
    {
        GetWindow<CubeSpawnerEditor>("Cube Spawner");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        spawnedCubes = new List<GameObject>();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Cube Spawner", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear Cubes"))
        {
            ClearCubes();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(worldRay, out hitInfo))
            {
                Vector3 spawnPosition = hitInfo.point + hitInfo.normal;
                SpawnCube(spawnPosition, hitInfo.normal);
                e.Use();
            }
        }

		// when we hover the selected object, increase its scale a bit
		if (Selection.activeGameObject != null)
		{
			Vector3 originalScale = Selection.activeGameObject.transform.localScale;
			Selection.activeGameObject.transform.localScale = originalScale * 1.2f;
			// clamp the scale to 1.2f
			if (Selection.activeGameObject.transform.localScale.x > 1.2f)
			{
				Selection.activeGameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			}
		}
    }

    private void SpawnCube(Vector3 position, Vector3 direction)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.rotation = Quaternion.LookRotation(direction);
        spawnedCubes.Add(cube);
    }

    private void ClearCubes()
    {
        foreach (GameObject cube in spawnedCubes)
        {
            if (cube != null)
            {
                DestroyImmediate(cube);
            }
        }

        spawnedCubes.Clear();
    }
}
