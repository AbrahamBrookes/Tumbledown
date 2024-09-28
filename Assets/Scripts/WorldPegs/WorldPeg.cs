using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tumbledown
{
	/**
	 * A WorldPeg is a single 100x100x100 cube in our game world. It is used to build out the maps
	 * in the game. A WorldPeg is part of a WorldPegGroup. The WorldPegGroup handles group-level
	 * things like the theme of the pegs in the group - each peg then has its own UI where you can
	 * modify peg-level things like, ramp or no ramp.
	 */
    public class WorldPeg : MonoBehaviour
    {
		// the WorldPegGroup to which this WorldPeg belongs
		[SerializeField] private WorldPegGroup _worldPegGroup;

		// allow peeking at the world peg group
		public WorldPegGroup WorldPegGroup { get { return _worldPegGroup; } }

		// the prefab we will render
		[SerializeField] private GameObject _prefabToSpawn = default;

		// allow peeking at the prefab
		public GameObject PrefabToSpawn { get { return _prefabToSpawn; } }

		// the rendered instance of the prefab
		[SerializeField] private GameObject _spawnedPrefab = default;

		// our offset - used to find our factory in the group
		[SerializeField] private Vector3Int _offset;

		// allow peeking at the offset
		public Vector3Int Offset { get { return _offset; } }

		// our worldPegLocation
		[SerializeField] private WorldPegLocation _worldPegLocation;

		// Init for passing in a world peg group and a prefab to spawn
		public void Init(WorldPegGroup worldPegGroup, GameObject prefabToSpawn, Vector3Int offset, WorldPegLocation worldPegLocation)
		{
			// set the world peg group
			_worldPegGroup = worldPegGroup;

			// set the prefab to spawn
			_prefabToSpawn = prefabToSpawn;

			// set the offset
			_offset = offset;

			// set the world peg location
			_worldPegLocation = worldPegLocation;
		}

		// a function to render the world peg
		public void Render()
		{
			// if we don't have an offset, we can't render
			if (_offset == null)
			{
				Debug.LogError("WorldPeg.Render() - no offset");
				return;
			}

			// this worldpeg shouldn't do any actual rendering, ask the group to pump a factory
			_worldPegGroup.SpawnWorldPeg(_offset.x, _offset.y, _offset.z);
		}

		// clear for re-rendering
		public void Clear()
		{
			// this worldpeg shouldn't do any actual clearing, ask the group to clear a factory
			// _worldPegGroup.ClearWorldPeg(_offset.x, _offset.y, _offset.z);
		}	
    }


	/**
	 * A WorldPeg has a custom editor interface where you can tell it to re-render itself
	 */
	[CustomEditor(typeof(WorldPeg))]
	public class WorldPegEditor : Editor
	{
		// the world peg we are editing
		private WorldPeg _worldPeg;

		// the prefab we will render
		private GameObject _prefabToSpawn;

		// on enable, get the world peg
		private void OnEnable()
		{
			// get the world peg
			_worldPeg = (WorldPeg)target;

			// get the prefab to spawn
			_prefabToSpawn = _worldPeg.PrefabToSpawn;
		}

		// render the inspector
		public override void OnInspectorGUI()
		{
			// draw the default inspector
			DrawDefaultInspector();

			// if we have a world peg
			if (_worldPeg != null)
			{
				// if the user clicks the render button
				if (GUILayout.Button("Render"))
				{
					// render the world peg
					_worldPeg.Render();
				}
			}

			// whenever the prefab of the world peg changes
			if (_prefabToSpawn != _worldPeg.PrefabToSpawn)
			{
				// // get the factory from the group
				// WorldPegFactory factory = _worldPeg.WorldPegGroup.GetFactory(_worldPeg.Offset);

				// // set the prefab to spawn
				// factory.PrefabToSpawn = _worldPeg.PrefabToSpawn;

				// // pump the factory
				// factory.Render();
			}
		}
	}

}
