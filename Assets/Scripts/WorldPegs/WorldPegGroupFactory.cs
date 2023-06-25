using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown
{
	public class WorldPegGroupFactory
	{
		// the theme of the world peg group
		private WorldPegGroupTheme _worldPegGroupTheme;

		// the vec3int size of the world peg group
		private Vector3Int _size;

		// the iterative mesh we'll build
		private CombineInstance[] _combineInstances = new CombineInstance[2];

		// the actual mesh we'll join onto
		private Mesh _joinMesh = new Mesh();

		// the matrices for like stuff i dunno
		List<Matrix4x4> matrices = new List<Matrix4x4>();

		// on construct, take a WorldPegGroupTheme and size
		public WorldPegGroupFactory(WorldPegGroupTheme worldPegGroupTheme, Vector3Int size)
		{
			_worldPegGroupTheme = worldPegGroupTheme;
			_size = size;
		}

		/**
		 * Create a combined mesh from all of our editor-time world peg meshes.
		 */
		public Mesh CreateCombinedMesh()
		{
			int index = 0;
			// loop through the x-y-z of the size
			for (int x = 0; x < _size.x; x++)
			{
				for (int y = 0; y < _size.y; y++)
				{
					for (int z = 0; z < _size.z; z++)
					{
						// get the offset
						Vector3Int offset = new Vector3Int(x, y, z);
						
						// figure out which mesh to use
						Mesh mesh = null;
						
						// if we're on the top, use a top mesh
						if (y == _size.y - 1)
						{
							mesh = _worldPegGroupTheme.topMeshes[Random.Range(0, _worldPegGroupTheme.topMeshes.Count)];
						}

						// if we're on the left, right, front or back, use a wall mesh
						else if (x == 0 || x == _size.x - 1 || z == 0 || z == _size.z - 1)
						{
							mesh = _worldPegGroupTheme.wallMeshes[Random.Range(0, _worldPegGroupTheme.wallMeshes.Count)];
						}

						// if we're at a corner on the top, use a top corner mesh
						else if (y == _size.y - 1 && (x == 0 || x == _size.x - 1 || z == 0 || z == _size.z - 1))
						{
							mesh = _worldPegGroupTheme.topCornerMeshes[Random.Range(0, _worldPegGroupTheme.topCornerMeshes.Count)];
						}

						// if we're at a corner not on the top, use a wall corner mesh
						else if (x == 0 || x == _size.x - 1 || z == 0 || z == _size.z - 1)
						{
							mesh = _worldPegGroupTheme.wallCornerMeshes[Random.Range(0, _worldPegGroupTheme.wallCornerMeshes.Count)];
						}

						// combine the mesh
						CombineMesh(mesh, offset, index);
						
						index ++;
					}
				}
			}

			// return the mesh
			return _joinMesh;
		}

		/**
		 * Given a mesh, combine that mesh into our combineinstance
		 */
		private void CombineMesh(Mesh mesh, Vector3Int offset, int index)
		{
			// if mesh is null no can do
			if (mesh == null)
			{
				Debug.LogError("Mesh is null, cannot combine");
				return;
			}

			// create a new mesh
			Mesh newMesh = new Mesh();
			newMesh.vertices = mesh.vertices;
			newMesh.normals = mesh.normals;
			newMesh.uv = mesh.uv;
			newMesh.triangles = mesh.triangles;
			newMesh.RecalculateBounds();

			// create a new combine instance
			_combineInstances[0].mesh = newMesh;
			_combineInstances[0].transform = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);

			// add the combine instance to the list of matrices
			matrices.Add(_combineInstances[0].transform);

			// create a new mesh from our _joinMesh
			Mesh newJoinMesh = new Mesh();
			newJoinMesh.vertices = _joinMesh.vertices;
			newJoinMesh.normals = _joinMesh.normals;
			newJoinMesh.uv = _joinMesh.uv;
			newJoinMesh.triangles = _joinMesh.triangles;
			newJoinMesh.RecalculateBounds();

			// create a new combine instance
			_combineInstances[1].mesh = newJoinMesh;
			_combineInstances[1].transform = Matrix4x4.identity;

			// add the combine instance to the list of matrices
			matrices.Add(_combineInstances[1].transform);

			// combine the meshes
			_joinMesh.CombineMeshes(_combineInstances, true, false);
		}
	}
}