using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tumbledown.WorldPegsTests 
{
	/**
		Our WorldPegGroup script spawns a bunch of blocks for the player to walk on, in a block, based
		on its private _size variable. This test ensures that the WorldPegGroup is spawning the correct
		number of blocks and that they are positioned correctly.
	*/
	public class WorldPegsTest
	{
		// this test checks that the world peg group is spawning the correct number of world pegs
		[UnityTest]
		public IEnumerator SpawnWorldPegsTest()
		{
			// create a new gameobject at 0,0,0
			var gameObject = new GameObject();

			// add a world peg group script to the gameobject
			var worldPegGroup = gameObject.AddComponent<Tumbledown.WorldPegGroup>();

			// set the size of the world peg group to 3,3,3
			worldPegGroup.Size = new Vector3(3, 3, 3);

			// spawn the world pegs
			worldPegGroup.SpawnWorldPegs();

			// wait for 1 second
			yield return new WaitForSeconds(1);

			// check that the world peg group has 27 children
			Assert.AreEqual(27, worldPegGroup.transform.childCount);

			// check that the world peg group's children are positioned correctly
			Assert.AreEqual(new Vector3(-1, -1, -1), worldPegGroup.transform.GetChild(0).transform.position);
			Assert.AreEqual(new Vector3(0, -1, -1), worldPegGroup.transform.GetChild(1).transform.position);
			Assert.AreEqual(new Vector3(1, -1, -1), worldPegGroup.transform.GetChild(2).transform.position);
			Assert.AreEqual(new Vector3(-1, 0, -1), worldPegGroup.transform.GetChild(3).transform.position);
			Assert.AreEqual(new Vector3(0, 0, -1), worldPegGroup.transform.GetChild(4).transform.position);
			Assert.AreEqual(new Vector3(1, 0, -1), worldPegGroup.transform.GetChild(5).transform.position);
			Assert.AreEqual(new Vector3(-1, 1, -1), worldPegGroup.transform.GetChild(6).transform.position);
			Assert.AreEqual(new Vector3(0, 1, -1), worldPegGroup.transform.GetChild(7).transform.position);
			Assert.AreEqual(new Vector3(1, 1, -1), worldPegGroup.transform.GetChild(8).transform.position);
			Assert.AreEqual(new Vector3(-1, -1, 0), worldPegGroup.transform.GetChild(9).transform.position);
			Assert.AreEqual(new Vector3(0, -1, 0), worldPegGroup.transform.GetChild(10).transform.position);
			Assert.AreEqual(new Vector3(1, -1, 0), worldPegGroup.transform.GetChild(11).transform.position);
			Assert.AreEqual(new Vector3(-1, 0, 0), worldPegGroup.transform.GetChild(12).transform.position);
			Assert.AreEqual(new Vector3(0, 0, 0), worldPegGroup.transform.GetChild(13).transform.position);
			Assert.AreEqual(new Vector3(1, 0, 0), worldPegGroup.transform.GetChild(14).transform.position);
			Assert.AreEqual(new Vector3(-1, 1, 0), worldPegGroup.transform.GetChild(15).transform.position);
			Assert.AreEqual(new Vector3(0, 1, 0), worldPegGroup.transform.GetChild(16).transform.position);
			Assert.AreEqual(new Vector3(1, 1, 0), worldPegGroup.transform.GetChild(17).transform.position);
			Assert.AreEqual(new Vector3(-1, -1, 1), worldPegGroup.transform.GetChild(18).transform.position);
			Assert.AreEqual(new Vector3(0, -1, 1), worldPegGroup.transform.GetChild(19).transform.position);
			Assert.AreEqual(new Vector3(1, -1, 1), worldPegGroup.transform.GetChild(20).transform.position);
			Assert.AreEqual(new Vector3(-1, 0, 1), worldPegGroup.transform.GetChild(21).transform.position);
			Assert.AreEqual(new Vector3(0, 0, 1), worldPegGroup.transform.GetChild(22).transform.position);
			Assert.AreEqual(new Vector3(1, 0, 1), worldPegGroup.transform.GetChild(23).transform.position);
			Assert.AreEqual(new Vector3(-1, 1, 1), worldPegGroup.transform.GetChild(24).transform.position);
			Assert.AreEqual(new Vector3(0, 1, 1), worldPegGroup.transform.GetChild(25).transform.position);
			Assert.AreEqual(new Vector3(1, 1, 1), worldPegGroup.transform.GetChild(26).transform.position);
		}
	}
}
