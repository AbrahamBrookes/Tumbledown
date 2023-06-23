using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tumbledown
{
	/**
	 * WorldPegGroup is a group of world pegs. It is responsible for spawning the world pegs
	 * and setting their collision cube. In order to render individual pegs, we'll use a
	 * 3d dictionary of WorldPegFactories which we can adjust individually, serialise etc
	 */
	public class WorldPegGroup : MonoBehaviour
	{
		// the world peg prefabs we will spawn
		[SerializeField] private List<GameObject> _worldPegPrefabs = new List<GameObject>();

		// a vec3 defining the size of the world peg group
		[SerializeField] private Vector3Int _size = new Vector3Int(2,2,2);

		// a dictionary indexed by vec3 containing the world peg factories we'll be using
		private Dictionary<Vector3Int, WorldPegFactory> _worldPegFactories = new Dictionary<Vector3Int, WorldPegFactory>();

		// allow setting _size
		public Vector3Int Size
		{
			get { return _size; }
			set { _size = value; }
		}

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

			// get the factory at x y z
			WorldPegFactory factory = GetFactory(x, y, z);

			// pump the factory
			factory.Render();
		}

		// clear a world peg
		public void ClearWorldPeg(int x, int y, int z)
		{
			// get the factory at x y z
			WorldPegFactory factory = GetFactory(x, y, z);

			// clear the factory
			factory.Clear();
		}

		// get a factory at x y z
		public WorldPegFactory GetFactory(int x, int y, int z)
		{
			// create a vec3 from x y z
			Vector3Int offset = new Vector3Int(x, y, z);

			// if the dictionary doesn't contain the key
			if (!_worldPegFactories.ContainsKey(offset))
			{
				// create a new factory
				_worldPegFactories.Add(offset, new WorldPegFactory(this, _worldPegPrefabs[Random.Range(0, _worldPegPrefabs.Count)], offset));
			}

			// return the factory
			return _worldPegFactories[offset];
		}

		// overload for get factory
		public WorldPegFactory GetFactory(Vector3Int offset)
		{
			return GetFactory(offset.x, offset.y, offset.z);
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
				// spawn the world pegs
				_worldPegGroup.SpawnWorldPegs();
			}
		}
	}
}
