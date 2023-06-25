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
		// the world peg group theme we'll use to select meshes
		[SerializeField] private WorldPegGroupTheme _worldPegGroupTheme;

		// a vec3 defining the size of the world peg group
		[SerializeField] private Vector3Int _size = new Vector3Int(2,2,2);

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
			// delete all child gameobjects
			foreach (Transform child in transform)
			{
				DestroyImmediate(child.gameObject);
			}

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
			// get the WorldPegLocation
			WorldPegLocation worldPegLocation = GetWorldPegLocation(x, y, z);

			// only if one of the flags is true, otherwise we are looking at an internal peg
			// create a blank location to compare against
			if( worldPegLocation.Equals(new WorldPegLocation()) )
			{
				return;
			}

			// figure out which mesh list to render from
			List<Mesh> meshType = GetMesh(worldPegLocation);

			// get a random mesh from the list
			Mesh selectedMesh = meshType[Random.Range(0, meshType.Count)];

			// create a new gameobject
			GameObject newGameObject = new GameObject();

			// set this as parent
			newGameObject.transform.parent = transform;

			// offset it
			newGameObject.transform.localPosition = new Vector3(x, y, z);

			// scale by 100
			newGameObject.transform.localScale = new Vector3(100, 100, 100);

			// translate -100 on local x
			newGameObject.transform.Translate(-1, 0, 0, Space.Self);

			// get the rotation from the world peg location
			Quaternion rotation = GetDesiredRotation(worldPegLocation);

			// set the rotation
			newGameObject.transform.localRotation = rotation;

			// give it a mesh filter
			MeshFilter meshFilter = newGameObject.AddComponent<MeshFilter>();

			// give it a mesh renderer
			MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer>();

			// set the mesh
			meshFilter.mesh = selectedMesh;

			// set the material
			meshRenderer.material = _worldPegGroupTheme.Material;

			// add a WorldPeg component for editing in the inspector
			WorldPeg worldPeg = newGameObject.AddComponent<WorldPeg>();

			// init the worldpeg
			worldPeg.Init(this, newGameObject, new Vector3Int(x, y, z), worldPegLocation);
		}

		// given an xyz, return the WorldPegLocation
		public WorldPegLocation GetWorldPegLocation(int x, int y, int z)
		{
			// flags for selecting the face type - ledge, wall, top, top corner, wall corner
			bool isLeft = false;
			bool isRight = false;
			bool isFront = false;
			bool isBack = false;
			bool isTop = false;

			// if the x offset is 0, we're on the left
			if (x == 0)
			{
				isLeft = true;
			}

			// if the x offset is the size of the world peg group, we're on the right
			if (x == _size.x - 1)
			{
				isRight = true;
			}

			// if the z offset is 0, we're on the front
			if (z == 0)
			{
				isFront = true;
			}

			// if the z offset is the size of the world peg group, we're on the back
			if (z == _size.z - 1)
			{
				isBack = true;
			}

			// if the y offset is 1 less than the size of the world peg group, we're on the top
			if (y == _size.y - 1)
			{
				isTop = true;
			}

			// create a new WorldPegLocation
			WorldPegLocation worldPegLocation = new WorldPegLocation();

			// set the flags
			worldPegLocation.isLeft = isLeft;
			worldPegLocation.isRight = isRight;
			worldPegLocation.isFront = isFront;
			worldPegLocation.isBack = isBack;
			worldPegLocation.isTop = isTop;

			// return the WorldPegLocation
			return worldPegLocation;
		}
		
		// figure out which mesh to render
		public List<Mesh> GetMesh(WorldPegLocation location)
		{
			// get the theme
			WorldPegGroupTheme theme = _worldPegGroupTheme;

			// our best bet at which tile to return
			List<Mesh> bestBet = new List<Mesh>();

			// best bet at cascading rules
			if (location.isTop)
			{
				bestBet = theme.topMeshes;

				if (location.isLeft || location.isRight)
				{
					bestBet = theme.ledgeMeshes;

					if (location.isFront || location.isBack)
					{
						bestBet = theme.topCornerMeshes;
					}
				}

				if (location.isFront || location.isBack)
				{
					bestBet = theme.ledgeMeshes;
				}
			}
			else
			{
				bestBet = theme.wallMeshes;

				if (location.isLeft || location.isRight)
				{
					if (location.isFront || location.isBack)
					{
						bestBet = theme.wallCornerMeshes;
					}
				}
			}

			// return the best bet
			return bestBet;
		}
		
		// figure out the rotation for the given location
		public Quaternion GetDesiredRotation(WorldPegLocation location)
		{
			// our best bet at which tile to return
			Quaternion bestBet = Quaternion.identity;

			// best bet at cascading rules
			if (location.isTop)
			{
				if (location.isLeft)
				{
					// rotate 90
					bestBet = Quaternion.Euler(0, 90, 0);

					if (location.isFront)
					{
						// rotate 0 degrees
						bestBet = Quaternion.Euler(0, 0, 0);
					}
					if (location.isBack)
					{
						// rotate 180 degrees
						bestBet = Quaternion.Euler(0, 90, 0);
					}
				}
				if (location.isRight)
				{
					// rotate 270
					bestBet = Quaternion.Euler(0, 270, 0);

					if (location.isFront)
					{
						// rotate 270 degrees
						bestBet = Quaternion.Euler(0, 270, 0);
					}
					if (location.isBack)
					{
						// rotate 180 degrees
						bestBet = Quaternion.Euler(0, 180, 0);
					}
				}

				if (!location.isLeft && !location.isRight)
				{
					if (location.isFront)
					{
						// rotate 0
						bestBet = Quaternion.identity;
					}

					if (location.isBack)
					{
						// rotate 180
						bestBet = Quaternion.Euler(0, 180, 0);
					}
				}
			}
			else
			{
				if (location.isLeft)
				{
					// rotate 90
					bestBet = Quaternion.Euler(0, 90, 0);

					if (location.isFront)
					{
						// rotate 0 degrees
						bestBet = Quaternion.Euler(0, 0, 0);
					}
					if (location.isBack)
					{
						// rotate 180 degrees
						bestBet = Quaternion.Euler(0, 90, 0);
					}
				}
				if (location.isRight)
				{
					// rotate 270
					bestBet = Quaternion.Euler(0, 270, 0);

					if (location.isFront)
					{
						// rotate 270 degrees
						bestBet = Quaternion.Euler(0, 270, 0);
					}
					if (location.isBack)
					{
						// rotate 180 degrees
						bestBet = Quaternion.Euler(0, 180, 0);
					}
				}

				if (!location.isLeft && !location.isRight)
				{
					if (location.isFront)
					{
						// no rotation needed
						bestBet = Quaternion.identity;
					}

					if (location.isBack)
					{
						// rotate 180
						bestBet = Quaternion.Euler(0, 180, 0);
					}
				}
			}

			// return the best bet
			return bestBet;
		}

		// depending on the location we may need to additionally offset some of the meshes
		// public Vector3 GetDesiredMeshOffset(WorldPegLocation location)
		// {
			
		// }

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
