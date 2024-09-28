using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;


/// <summary>
/// The TerrainBrush class extends Unity's GridBrushBase to facilitate the automated placement of terrain tiles
/// in a grid-based environment using the Marching Squares algorithm. This brush allows for the manual placement
/// of top tiles, while automatically determining and placing the appropriate edge, outer corner, and inner corner
/// tiles based on the surrounding terrain configuration.
/// 
/// The Marching Squares algorithm is implemented here by defining a 4-bit mask for each grid cell, representing
/// the state of each of its four corners. This mask is then used to look up the correct GameObject and its rotation
/// from a predefined lookup table, allowing for the correct terrain GameObject to be instantiated with the appropriate
/// rotation to match the surrounding terrain.
/// 
/// Public Fields:
/// - topTile: The GameObject representing a top tile.
/// - edgeTile: The GameObject representing an edge tile.
/// - outerCornerTile: The GameObject representing an outer corner tile.
/// - innerCornerTile: The GameObject representing an inner corner tile.
/// 
/// Methods:
/// - Paint: Allows for the manual placement of top tiles, updating the grid state and invoking the Marching Squares
///          algorithm to update the surrounding tiles accordingly.
/// - Erase: Allows for the removal of top tiles, updating the grid state and invoking the Marching Squares algorithm
///          to update the surrounding tiles accordingly.
/// - UpdateSurroundingTiles: Updates all tiles surrounding a given tile based on the current grid state.
/// - UpdateTile: Updates a single tile based on its current state and the state of its neighboring tiles, determining
///               the correct GameObject and rotation to use.
/// - GetMask: Calculates the 4-bit mask representing the state of a cell's corners based on the current grid state.
/// 
/// Usage:
/// Attach this script to a GridBrush in your Unity project and assign the appropriate GameObjects to the public fields
/// to enable the automated placement of terrain tiles using the Marching Squares algorithm.
/// 
/// Note:
/// This script is a starting point and will require further development and testing to handle all possible terrain
/// configurations correctly, including the definition of a lookup table in the UpdateTile method.
/// </summary>


[CustomGridBrush(true, false, false, "Terrain Brush")]
public class TerrainBrush : GridBrushBase
{
    public GameObject topTile;
    public GameObject edgeTile;
    public GameObject outerCornerTile;
    public GameObject innerCornerTile;

	private GridLayout _gridLayout;

	// how far to offset spawned tiles to make up for offset origins so we can rotate sensibly
	private Vector3 _tileOffset = new Vector3(0.5f, 0f, 0.5f);

	/**
	 * @param gridLayout The grid layout that the brush is painting on.
	 * @param brushTarget The target of the brush (can be null).
	 * @param position The cell position of the brush. (in grid space, not world space - convert using gridLayout.CellToWorld(position))
	 */
    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {	
		_gridLayout = gridLayout;
		
		// destroy any existing tiles at this position and add a top tile instead
		Vector3 worldPos = _gridLayout.CellToWorld(position);
		GameObject tile = TileAtWorldPosition(worldPos);

		if (tile != null)
			DestroyImmediate(tile);

		// instantiate a top tile
		GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(topTile);
		instance.transform.SetParent(_gridLayout.transform);
		instance.transform.position = _gridLayout.CellToWorld(position) + _tileOffset;

		// call the new tile "top"
		instance.name = "top";

        UpdateSurroundingTiles(position);
    }

    private void UpdateSurroundingTiles(Vector3Int position)
    {
		// for each of the surrounding 8 positions
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				// if the position is not the center position
				if (x != 0 || y != 0)
				{
					// update the tile at that position
					UpdateTile(position + new Vector3Int(x, y, 0));
				}
			}
		}
    }

	/**
	 * Given a world position, return the tile game object at that position.
	 */
	public GameObject TileAtWorldPosition(Vector3 worldPos)
	{
		// loop through every child of the grid layout
		foreach (Transform child in _gridLayout.transform)
		{
			// if the child's position is the same as the given world position
			if (child.position == worldPos + _tileOffset)
			{
				return child.gameObject;
			}
		}

		return null;
	}

    private void UpdateTile(Vector3Int position)
    {
		// loop around all surrounding positions and create a dictionary of compass directions and if there is a tile there
		Dictionary<CompassDirection, bool> surroundingTiles = new Dictionary<CompassDirection, bool>();
		foreach (CompassDirection direction in System.Enum.GetValues(typeof(CompassDirection)))
		{
			Vector3Int surroundingPosition = position - CompassDirectionToVector(direction);
			surroundingTiles[direction] = TileAtWorldPosition(_gridLayout.CellToWorld(surroundingPosition)) != null;
		}

		// serialize and log the surrounding tiles
		string surroundingTilesString = "";
		foreach (KeyValuePair<CompassDirection, bool> entry in surroundingTiles)
		{
			// eg "N:T,E:F,S:F,W:F,NE:F,SE:F,SW:F,NW:T"
			surroundingTilesString += entry.Key + ":" + (entry.Value ? "T" : "F") + ",";
		}

		// a hard-coded lookup table of all possible tile configurations and the tile to use for each
		Dictionary<string, Dictionary<GameObject, Quaternion>> lookupTable = new Dictionary<string, Dictionary<GameObject, Quaternion>>();
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:F,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:F,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:T,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:T,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:T,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:F,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:F,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:F,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:F,S:F,W:F,NE:T,SE:T,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:T,S:F,W:F,NE:F,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:T,S:F,W:F,NE:T,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:F,E:T,S:F,W:F,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:F,E:T,S:F,W:F,NE:F,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:T,S:F,W:F,NE:T,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:T,W:F,NE:F,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:F,E:F,S:T,W:F,NE:T,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:T,W:F,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:F,E:F,S:T,W:F,NE:F,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:T,W:F,NE:T,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:T,NE:F,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:F,E:F,S:F,W:T,NE:T,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:F,E:F,S:F,W:T,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { edgeTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:T,NE:F,SE:F,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:T,NE:T,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:F,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:T,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:F,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { outerCornerTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:T,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 90) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:F,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 180) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:F,SW:T,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 270) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:T,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { innerCornerTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:F,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:T,SW:F,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:T,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:F,E:F,S:F,W:F,NE:F,SE:T,SW:F,NW:F,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };
		lookupTable["N:T,E:T,S:T,W:T,NE:T,SE:T,SW:T,NW:T,"] = new Dictionary<GameObject, Quaternion>() { { topTile, Quaternion.Euler(0, 0, 0) } };

		// Debug.Log("looking for: " + surroundingTilesString);
		// using the lookup table, determine the correct tile and rotation to use
		GameObject tile = null;
		Quaternion rotation = Quaternion.identity;
		if (lookupTable.ContainsKey(surroundingTilesString))
		{
		Debug.Log("found: " + surroundingTilesString);
			Dictionary<GameObject, Quaternion> tileAndRotation = lookupTable[surroundingTilesString];
			foreach (KeyValuePair<GameObject, Quaternion> entry in tileAndRotation)
			{
				tile = entry.Key;
				rotation = entry.Value;
			}
		}

		// if a tile was found
		if (tile != null)
		{
			// destroy any existing tile at this position
			Vector3 worldPos = _gridLayout.CellToWorld(position);
			GameObject existingTile = TileAtWorldPosition(worldPos);
			if (existingTile != null)
				DestroyImmediate(existingTile);

			// instantiate the new tile
			GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(tile);
			instance.transform.SetParent(_gridLayout.transform);
			instance.transform.position = _gridLayout.CellToWorld(position) + _tileOffset;
			instance.transform.rotation = rotation;

			// call the new tile "edge"
			instance.name = "edge";
		}
		else
		{
			// if no tile was found, destroy any existing tile at this position
			// Vector3 worldPos = _gridLayout.CellToWorld(position);
			// GameObject existingTile = TileAtWorldPosition(worldPos);
			// if (existingTile != null)
			// 	DestroyImmediate(existingTile);
		}
	}
	
	// an enum with all CompassDirections
	public enum CompassDirection
	{
		N,E,S,W,NE,SE,SW,NW
	}

	/**
	 * Given a compass direction, return the vector that represents that direction.
	 */
	public Vector3Int CompassDirectionToVector(CompassDirection direction)
	{
		switch (direction)
		{
			case CompassDirection.N:
				return new Vector3Int(0, 1, 0);
			case CompassDirection.E:
				return new Vector3Int(1, 0, 0);
			case CompassDirection.S:
				return new Vector3Int(0, -1, 0);
			case CompassDirection.W:
				return new Vector3Int(-1, 0, 0);
			case CompassDirection.NE:
				return new Vector3Int(1, 1, 0);
			case CompassDirection.SE:
				return new Vector3Int(1, -1, 0);
			case CompassDirection.SW:
				return new Vector3Int(-1, -1, 0);
			case CompassDirection.NW:
				return new Vector3Int(-1, 1, 0);
			default:
				return Vector3Int.zero;
		}
	}

}
