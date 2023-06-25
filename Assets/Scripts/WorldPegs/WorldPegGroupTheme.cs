using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown
{
	/**
	 * A struct for describing the theme of a WorldPegGroup - Gameojects with meshes and
	 * materials (all the same material) that show the corners, edges, tops walls of a 
	 * WorldPegGroup - these meshes will be fed to the WorldPegGroupFactory to create a
	 * combined mesh. Each 'mesh' is a list of meshes to select from at random, for the
	 * sake of some variety.
	 */
    [CreateAssetMenu(fileName = "WorldPegGroupTheme", menuName = "Tumbledown/WorldPegGroupTheme")]
    public class WorldPegGroupTheme : ScriptableObject
    {
		public List<Mesh> topCornerMeshes;
		public List<Mesh> wallCornerMeshes;
		public List<Mesh> ledgeMeshes;
		public List<Mesh> wallMeshes;
		public List<Mesh> topMeshes;
		public Material Material;
	}
}