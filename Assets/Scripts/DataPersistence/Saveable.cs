using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.DataPersistence
{
    public class Saveable : MonoBehaviour
    {
		// the filename of the file on disk that we will save to and load from
		[SerializeField] private string _saveFileName;
		// allow set and get of the data file name
		public string SaveFileName { get => _saveFileName; set => _saveFileName = value; }

		// the save data type we will be loading and saving
		[SerializeField] private Type _saveData;

		// save the data using our FileDataHandler class
		public void Save(SaveData data, bool encrypt = true)
		{
			// check we have a save file name
			if (string.IsNullOrEmpty(_saveFileName))
			{
				Debug.LogError("Saveable: SaveFileName is null or empty.");
				return;
			}

			// check the incoming data is workable
			if (data == null)
			{
				Debug.LogError("Saveable: SaveData is null.");
				return;
			}

			// create a new file data handler
			FileDataHandler dataHandler = new FileDataHandler(_saveFileName, encrypt);

			// save the data
			dataHandler.Save(data);
		}

		// load the data using our FileDataHandler class
		public T Load<T>(bool encrypt = true)
		{
			// check we have a save file name
			if (string.IsNullOrEmpty(_saveFileName))
			{
				Debug.LogError("Saveable: SaveFileName is null or empty.");
				return default(T);
			}

			// create a new file data handler
			FileDataHandler dataHandler = new FileDataHandler(_saveFileName, encrypt);

			// load the data
			T data = dataHandler.Load<T>();

			return data;
		}

    }
}
