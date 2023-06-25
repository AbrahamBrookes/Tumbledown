using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown
{
	/**
	 * Our WorldPegFactory takes care of creating WorldPegs, destroying and re-creating them.
	 * It's not a monobehaviour, it's just a cllass and it survives the multiple destroyings
	 * and re-creatings of its WorldPeg companion.
	 */
	public class WorldPegFactory
	{
		// the world peg group to which this factory belongs
		private WorldPegGroup _worldPegGroup;

		// the prefab we will spawn
		private GameObject _prefabToSpawn;

		// allow setting the prefab to spawn
		public GameObject PrefabToSpawn { set { _prefabToSpawn = value; } }

		// the prefab we did spawn
		private GameObject _spawnedPrefab;

		// our offset as calculated by the WorldPeGroup
		private Vector3Int _offset;

		public WorldPegFactory(WorldPegGroup worldPegGroup, GameObject prefabToSpawn, Vector3Int offset)
		{
			// set the world peg group
			_worldPegGroup = worldPegGroup;

			// set the prefab to spawn
			_prefabToSpawn = prefabToSpawn;

			// set the offset
			_offset = offset;
		}

		// a function to render the world peg
		public void Render()
		{
			// if we don't have a world peg group, we can't render
			if (_worldPegGroup == null)
			{
				Debug.LogError("WorldPeg.Render() - no world peg group");
				return;
			}

			// if we don't have a prefab to spawn, we can't render
			if (_prefabToSpawn == null)
			{
				Debug.LogError("WorldPeg.Render() - no prefab to spawn");
				return;
			}

			// if we don't have an offset, we can't render
			if (_offset == null)
			{
				Debug.LogError("WorldPeg.Render() - no offset");
				return;
			}

			// clear any existing rendered prefab
			Clear();

			// add offset to the world peg group position
			Vector3 position = _worldPegGroup.transform.position + _offset;

			// instantiate the prefab
			_spawnedPrefab = GameObject.Instantiate(_prefabToSpawn, position, Quaternion.identity);

			// slap a world peg script on it
			WorldPeg worldPeg = _spawnedPrefab.AddComponent<WorldPeg>();

			// init the worldpeg
			// worldPeg.Init(_worldPegGroup, _prefabToSpawn, _offset);
		}

		// clear for re-rendering
		public void Clear()
		{
			// if we have spawned a prefab, destroy it
			if (_spawnedPrefab != null)
			{
				// destroy the spawned prefab
				GameObject.DestroyImmediate(_spawnedPrefab);

				// set the spawned prefab to null
				_spawnedPrefab = null;
			}
		}
	}
}
