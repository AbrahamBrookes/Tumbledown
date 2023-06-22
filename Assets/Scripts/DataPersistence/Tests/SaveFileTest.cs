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
	 * In our game we will need to save a lot of different things at different times. This test
	 * shows how to use the save system in this project. You wull need to implement the save logic
	 * yourself, in your components, using the SaveFile class. In order to save something, you'll
	 * first need a file to save to:
	 *
	 * private SaveFile _savefile = new SaveFile("my_save"); // initialize the class, doesn't save anything yet
	 * private MySaveData _saveData = _saveFile.Load<MySaveData>(); // load or create the save file
	 * _saveData.someValue = 5; // change the save data
	 * _saveFile.Save<MySaveData>(_saveData); // save the save data back to the file
	 *
	 * This will create a new file - "my_save.json" - in the persistent data path of the game, or
	 * load one if it exists. Try to do the loading somewhere that a frame stutter won't be noticed,
	 * like in Awake or Start, or in response to a UI event. And keep your files small! You don't
	 * want to be loading a 10MB file every time you want to save something.
	 *
	 * Under the hood it's using NewtonSoft's JSON serialiser so you should be able to throw most
	 * things at it and it'll work.
	 *
	 * For your serialisable types, I'm using SomethingSomethingSaveData as a naming convention, and
	 * storing the save data classes right next to whatever they serialise. It's not a good idea to
	 * serialise your components, you're better off making a class that has just the fields you want
	 * to save, and save and load from that.
	 */
    public class SaveFileTest
    {
		/**
		 * a string - the name of the file we'll be saving to
		 */
		private string _saveFileName = "testSaveFile";

		/**
		 * On Teardown, make sure to delete our test file
		 */
		[TearDown]
		public void Teardown()
		{
			//if it exists, 
			if (File.Exists(Application.persistentDataPath + "/" + _saveFileName + ".json"))
			{
				// delete it
				File.Delete(Application.persistentDataPath + "/" + _saveFileName + ".json");
			}
		}

		/**
		 * In order to create a save file, we need to create a SaveFile object. This will create
		 * a new file, or load an existing one.
		 */
		[Test]
		public void TestSaveFileIsCreatedOnDisk(){
			// create a new SaveFile object
			SaveFile saveFile = new SaveFile(_saveFileName);

			// the file should not exist yet
			Assert.IsFalse(File.Exists(Application.persistentDataPath + "/" + _saveFileName + ".json"));

			// load the file
			saveFile.Load<TestSaveData>();

			// check that the file exists
			Assert.IsTrue(File.Exists(Application.persistentDataPath + "/" + _saveFileName + ".json"));
		}

		/**
		 * We can save and load data
		 */
		[Test]
		public void TestSaveAndLoad(){
			// create a new SaveFile object
			SaveFile saveFile = new SaveFile(_saveFileName);

			// update some stuff
			saveFile.Save<TestSaveData>(new TestSaveData(4, "testing", 50.0f, false));

			// load it
			TestSaveData data = saveFile.Load<TestSaveData>();

			// check that the data is the same
			Assert.AreEqual(data.testInt, 4);
			Assert.AreEqual(data.testString, "testing");
			Assert.AreEqual(data.testFloat, 50.0f);
			Assert.AreEqual(data.testBool, false);
		}

		/**
		 * If a file exists, loading it should not overwrite it
		 */
		[Test]
		public void TestExistingFileIsNotOverwritten(){
			// create a new SaveFile object
			SaveFile saveFile = new SaveFile(_saveFileName);

			// load the file
			TestSaveData data = saveFile.Load<TestSaveData>();

			// update some stuff
			saveFile.Save<TestSaveData>(new TestSaveData(1, "test", 5.0f, true));

			// load it again
			TestSaveData data2 = saveFile.Load<TestSaveData>();

			// check that the data is the same
			Assert.AreEqual(data2.testInt, 1);
			Assert.AreEqual(data2.testString, "test");
			Assert.AreEqual(data2.testFloat, 5.0f);
			Assert.AreEqual(data2.testBool, true);

			// create a totally new SaveFile object
			SaveFile saveFile2 = new SaveFile(_saveFileName);

			// load the file
			TestSaveData data3 = saveFile2.Load<TestSaveData>();

			// check that the data is the same
			Assert.AreEqual(data3.testInt, 1);
			Assert.AreEqual(data3.testString, "test");
			Assert.AreEqual(data3.testFloat, 5.0f);
			Assert.AreEqual(data3.testBool, true);
		}


    }
}
