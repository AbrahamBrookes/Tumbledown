using UnityEditor;
using UnityEngine;

public class TileMapEditor : EditorWindow
{
    private GameObject[,] grid;
    private GameObject selectedTile;
    private Vector2Int gridSize = new Vector2Int(60, 40);
    private Vector2 scrollView;
    private Mesh[] availableMeshes;
    private int selectedMeshIndex = 0;

    [MenuItem("Window/Tile Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<TileMapEditor>("Tile Map Editor");
    }

    private void OnEnable()
    {
        InitializeGrid();
        LoadAvailableMeshes();
    }

    private void OnGUI()
    {
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        for (int y = 0; y < gridSize.y; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize.x; x++)
            {
                if (GUILayout.Button(grid[x, y] ? grid[x, y].name : "", GUILayout.Width(12), GUILayout.Height(12)))
                {
                    SelectTile(x, y);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (selectedTile)
        {
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            selectedMeshIndex = EditorGUILayout.Popup("Mesh", selectedMeshIndex, GetMeshNames());
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedTile.GetComponent<MeshFilter>(), "Changed Mesh");
                selectedTile.GetComponent<MeshFilter>().sharedMesh = availableMeshes[selectedMeshIndex];
            }

            EditorGUI.BeginChangeCheck();

            Vector3 newPosition = EditorGUILayout.Vector3Field("Position", selectedTile.transform.localPosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedTile.transform, "Changed Position");
                selectedTile.transform.localPosition = newPosition;
            }

            EditorGUI.BeginChangeCheck();

            Vector3 newRotation = EditorGUILayout.Vector3Field("Rotation", selectedTile.transform.localEulerAngles);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedTile.transform, "Changed Rotation");
                selectedTile.transform.localEulerAngles = newRotation;
            }
        }
    }

    private void InitializeGrid()
    {
        grid = new GameObject[gridSize.x, gridSize.y];
    }

    private void LoadAvailableMeshes()
    {
        // Load all meshes in the Resources folder into availableMeshes.
        availableMeshes = Resources.LoadAll<Mesh>("path");
    }

    private void SelectTile(int x, int y)
    {
        if (grid[x, y] == null)
        {
            grid[x, y] = CreateTileInstance(x, y);
        }

        selectedTile = grid[x, y];
    }

    private GameObject CreateTileInstance(int x, int y)
    {
        GameObject tile = new GameObject($"Tile ({x},{y})");
        MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = tile.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = availableMeshes[selectedMeshIndex];
		// set material index 0 to grass, 1 to dirt, 2 to stone
		meshRenderer.sharedMaterials = new Material[] { Resources.Load<Material>("path") };
		

		// set the tile to the location designated by its x and y coordinates
		tile.transform.localPosition = new Vector3(x, 0, y);

		// scale up by 10x
		tile.transform.localScale = Vector3.one * 100;

		// rotate 90 on the x
		tile.transform.localEulerAngles = new Vector3(-90, 0, 0);

        return tile;
    }

    private string[] GetMeshNames()
    {
        string[] meshNames = new string[availableMeshes.Length];
        for (int i = 0; i < availableMeshes.Length; i++)
        {
            meshNames[i] = availableMeshes[i].name;
        }
        return meshNames;
    }
}