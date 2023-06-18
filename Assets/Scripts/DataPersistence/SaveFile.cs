using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.DataPersistence
{
    public class SaveFile
    {
		// the filename of the file on disk that we will save to and load from
		[SerializeField] private string _saveFileName;
		// allow set and get of the data file name
		public string SaveFileName { get => _saveFileName; set => _saveFileName = value; }

		// our data handler for actually de/serializing the data
		private FileDataHandler _dataHandler;

		// on construction, cache the save file name
		public SaveFile(string saveFileName)
		{
			// saveFileName can not be null or empty
			if (string.IsNullOrEmpty(saveFileName))
			{
				Debug.LogError("SaveFile: SaveFileName is null or empty.");
				return;
			}

			_saveFileName = saveFileName;
			_dataHandler = new FileDataHandler();
		}

		// save the data using our FileDataHandler class
		public void Save(SaveData data)
		{
			// check the incoming data is workable
			if (data == null)
			{
				Debug.LogError("SaveFile: SaveData is null.");
				return;
			}

			// init the data handler
			_dataHandler.Init(_saveFileName);

			// save the data
			_dataHandler.Save(data);
		}

		// load the data using our FileDataHandler class
		public T Load<T>() where T : SaveData
		{
			// init the data handler
			_dataHandler.Init(_saveFileName);

			// check that save file exists
			if (!_dataHandler.SaveFileExists())
			{
				Debug.LogError("SaveFile: Save file does not exist.");
				return null;
			}

			// load the data
			T data = _dataHandler.Load<T>();

			return data;
		}

    }
}
