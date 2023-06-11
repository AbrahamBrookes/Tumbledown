using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldPegGroup : MonoBehaviour
{
	// the worldpegs we have spawned
	[SerializeField] private List<GameObject> _worldPegs = default;

	// the world peg prefabs we will spawn
	[SerializeField] private List<GameObject> _worldPegPrefabs = default;

	// a vec3 defining the size of the world peg group
	[SerializeField] private Vector3 _size = default;

	// we'll adapt our collision cube and offset it to fit the world peg group
	[SerializeField] private BoxCollider _collisionCube = default;

	// spawn the world pegs
	public void SpawnWorldPegs()
	{
		// loop through the x axis
		for (int x = 0; x < _size.x; x++)
		{
			// loop through the y axis
			for (int y = 0; y < _size.y; y++)
			{
				// loop through the z axis
				for (int z = 0; z < _size.z; z++)
				{
					// spawn a world peg
					SpawnWorldPeg(x, y, z);
				}
			}
		}

		// set collision
		SetCollisionCubeSize(_size.x, _size.y, _size.z);
	}

	// spawn a world peg
	public void SpawnWorldPeg(int x, int y, int z)
	{
		// get a random world peg prefab
		GameObject worldPegPrefab = _worldPegPrefabs[Random.Range(0, _worldPegPrefabs.Count)];

		// position will be relative to the transform
		Vector3 position = transform.position;
		position += new Vector3(x, y, z);

		// instantiate the world peg prefab
		GameObject worldPeg = Instantiate(worldPegPrefab, position, Quaternion.identity);
		// rotate 90 x
		worldPeg.transform.Rotate(-90, 0, 0);

		// set the world peg's parent to this transform
		worldPeg.transform.SetParent(transform);

		// add the world peg to the list of world pegs
		_worldPegs.Add(worldPeg);
	}

	// clear all pegs
	public void ClearWorldPegs()
	{
		// loop through the world pegs
		for (int i = 0; i < _worldPegs.Count; i++)
		{
			// destroy the world peg
			DestroyImmediate(_worldPegs[i]);
		}

		// clear the list of world pegs
		_worldPegs.Clear();
	}

	// set the size of the collision cube
	public void SetCollisionCubeSize(float x, float y, float z)
	{
		// if no box collider is set, create one
		_collisionCube = GetComponent<BoxCollider>();
		if (_collisionCube == null)
		{
			_collisionCube = gameObject.AddComponent<BoxCollider>();
		}

		// set the size of the collision cube
		_collisionCube.size = _size;

		// offset the collision cube to fit the world peg group
		_collisionCube.center = new Vector3((x / 2) - 0.5f, y / 2, (z / 2) - 0.5f);
	}
}


[CustomEditor(typeof(WorldPegGroup))]
public class WorldPegGroupEditor : Editor
{
	private WorldPegGroup _worldPegGroup;
	
    private void OnEnable()
    {
        _worldPegGroup = (WorldPegGroup)target;
    }
	
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        EditorGUILayout.LabelField("World Editor", EditorStyles.boldLabel);
		
		// if the button is pressed
		if (GUI.Button(new Rect(10, 10, 100, 30), "Re-render pegs"))
		{
			// clear the world pegs
			_worldPegGroup.ClearWorldPegs();

			// spawn the world pegs
			_worldPegGroup.SpawnWorldPegs();
		}
	}
}
