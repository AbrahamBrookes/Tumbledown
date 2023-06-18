using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tumbledown.DataPersistence;

namespace Tumbledown.DataPersistence.Tests
{
	/**
	 * In our game we will need to save a lot of different things at different times. In order to
	 * save something, add the `Saveable` component onto the gameobject, passing in the save file
	 * name to write to. When you want to save, call the `Save` method on the `Saveable` script,
	 * passing in the data you want to save. The Saveable component will add your data in to the
	 * specified file, or update it if a match is found.
	 */
    public class SaveableTest
    {
		/**
		 * On Teardown, make sure to delete our test file
		 */
		[TearDown]
		public void Teardown()
		{
			// delete our test file
			File.Delete(Application.persistentDataPath + "/testSaveFile.json");
		}

		/**
		 * Test that we are able to add a Saveable component to a gameobject and initialize it
		 * with a save file name.
		 */
		[Test]
		public void SaveableCanBeAddedToGameObject()
		{
			// arrange
			// create a new game object
			GameObject gameObject = new GameObject();

			// act
			// add a Saveable component to the game object
			Saveable saveable = gameObject.AddComponent<Saveable>();

			// set our save file name
			saveable.SaveFileName = "testSaveFile";

			// assert
			// check that the saveable component is not null
			Assert.IsNotNull(gameObject.GetComponent<Saveable>());
		}

		/**
		 * Test that we are able to save data to a file.
		 */
		[Test]
		public void SaveableCanSaveDataToFile()
		{
			// arrange
			// create a new game object
			GameObject gameObject = new GameObject();

			// add a Saveable component to the game object
			Saveable saveable = gameObject.AddComponent<Saveable>();

			// set our save file name
			saveable.SaveFileName = "testSaveFile";

			// create a new save data object
			TestSaveData testSaveData = new TestSaveData(259, "test string", 3.14f, true);

			// act
			// call the Save method on the Saveable component, passing in our save data
			saveable.Save(testSaveData, false);

			// assert
			// check that the file exists
			Assert.IsTrue(System.IO.File.Exists(Path.Combine(Application.persistentDataPath, saveable.SaveFileName)));
		}

		/**
		 * Test that we are able to load data from a file.
		 */
		[Test]
		public void SaveableCanLoadDataFromFile()
		{
			// arrange
			// create a new game object
			GameObject gameObject = new GameObject();

			// add a Saveable component to the game object
			Saveable saveable = gameObject.AddComponent<Saveable>();

			// set our save file name
			saveable.SaveFileName = "testSaveFile";

			// create a new save data object
			TestSaveData testSaveData = new TestSaveData(259, "test string", 3.14f, true);

			// create a new file data handler
			FileDataHandler dataHandler = new FileDataHandler(saveable.SaveFileName);

			// save the data
			dataHandler.Save(testSaveData);

			// act
			// call the Save method on the Saveable component, passing in our save data
			TestSaveData loadedData = (TestSaveData)saveable.Load<TestSaveData>();

			// assert
			// check that the loaded data is not null
			Assert.IsNotNull(loadedData);

			// check that the loaded data is the same as the saved data
			Assert.AreEqual(testSaveData.testInt, loadedData.testInt);
			Assert.AreEqual(testSaveData.testString, loadedData.testString);
			Assert.AreEqual(testSaveData.testFloat, loadedData.testFloat);
			Assert.AreEqual(testSaveData.testBool, loadedData.testBool);
		}


    }
}
