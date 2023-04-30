using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * WorldVolume is an editor tool for creating walkable ledges in the game. A WorldVolume has a height, 
 * width, and depth. As we set the dimensions, this script calculates the new size of the WorldVolume
 * and fills in each 100x100x100 area with a mesh. In order to introduce a bit of variation, the meshes
 * are 100x100x100, 100x100x200, 100x100x300, and 100x100x400. These meshes snap together to make a 
 * seamless walkable ledge. The WorldVolume also has a collider that matches the size of the WorldVolume.
 */

public class WorldVolume : MonoBehaviour
{
	// The size of the WorldVolume, calculated from the 8 widgets in the editor
	[SerializeField] private Vector3 _size = new Vector3(4, 4, 1);

	// The meshes we use to fill in the WorldVolume
	[SerializeField] private Mesh _mesh_ledge_100;
	[SerializeField] private Mesh _mesh_ledge_200;
	[SerializeField] private Mesh _mesh_ledge_300;
	[SerializeField] private Mesh _mesh_ledge_400;
	[SerializeField] private Mesh _mesh_ledge_corner;

	// wall straight meshes
	[SerializeField] private Mesh _mesh_wall_100;
	[SerializeField] private Mesh _mesh_wall_200;
	[SerializeField] private Mesh _mesh_wall_300;
	[SerializeField] private Mesh _mesh_wall_400;
	[SerializeField] private Mesh _mesh_wall_corner;

	// The collider for the WorldVolume
	[SerializeField] private BoxCollider _collider;

	// The list of meshes we use to fill in the WorldVolume
	[SerializeField] private List<Mesh> _meshes = new List<Mesh>();

	/**
	 * The _size is a Vector3 that represents the size of the WorldVolume. Each dimension is a multiple of 100.
	 * When we render the WorldVolume, we use the _size to calculate the number of meshes we need to fill in
	 * the WorldVolume. We also use the _size to calculate the size of the collider.
	 */
	public void fillMeshes()
	{
		// destroy all meshes
		foreach (Transform child in transform)
		{
			DestroyImmediate(child.gameObject);
		}

		// clear the list of meshes
		_meshes.Clear();

		// calculate the number of meshes we need to fill in the WorldVolume
		int numMeshes = (int)(_size.x * _size.y * _size.z);

		// add the meshes to the list
		for (int i = 0; i < numMeshes; i++)
		{
			// calculate the size of the mesh
			Vector3 size = new Vector3(1, 1, 1);
			size.x *= (i % _size.x) + 1;
			size.y *= (i % _size.y) + 1;
			size.z *= (i % _size.z) + 1;

			// add the mesh to the list
			if (size.z == 1)
			{
				_meshes.Add(_mesh_ledge_100);
			}
			else if (size.z == 2)
			{
				_meshes.Add(_mesh_ledge_200);
			}
			else if (size.z == 3)
			{
				_meshes.Add(_mesh_ledge_300);
			}
			else if (size.z == 4)
			{
				_meshes.Add(_mesh_ledge_400);
			}
			else
			{
				_meshes.Add(_mesh_ledge_corner);
			}
		}

		// add the wall meshes to the list
		for (int i = 0; i < numMeshes; i++)
		{
			// calculate the size of the mesh
			Vector3 size = new Vector3(1, 1, 1);
			size.x *= (i % _size.x) + 1;
			size.y *= (i % _size.y) + 1;
			size.z *= (i % _size.z) + 1;

			// add the mesh to the list
			if (size.z == 1)
			{
				_meshes.Add(_mesh_wall_100);
			}
			else if (size.z == 2)
			{
				_meshes.Add(_mesh_wall_200);
			}
			else if (size.z == 3)
			{
				_meshes.Add(_mesh_wall_300);
			}
			else if (size.z == 4)
			{
				_meshes.Add(_mesh_wall_400);
			}
			else
			{
				_meshes.Add(_mesh_wall_corner);
			}
		}

		// set the size of the collider
		_collider.size = _size;

		// instantiate the meshes
		for (int i = 0; i < _meshes.Count; i++)
		{
			// calculate the position of the mesh
			Vector3 position = new Vector3(0, 0, 0);
			position.x += (i % _size.x);
			position.y += ((i / _size.x) % _size.y);
			position.z += ((i / (_size.x * _size.y)) % _size.z);

			// instantiate the mesh
			GameObject mesh = new GameObject();
			mesh.transform.parent = transform;
			mesh.transform.localPosition = position;
			mesh.transform.localRotation = Quaternion.identity;
			mesh.transform.localScale = Vector3.one;
			mesh.AddComponent<MeshFilter>().mesh = _meshes[i];
			mesh.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
		}
	}

	/**
	 * Whenever we update size, we need to recalculate the meshes and the collider.
	 */
	public Vector3 size
	{
		get { return _size; }
		set 
		{
			_size = value;
			fillMeshes();
		}
	}

	/**
	 * A button on the editor that fills in the WorldVolume with meshes.
	 */
	[ContextMenu("Fill Meshes")]
	public void fillMeshesButton()
	{
		fillMeshes();
	}

}
