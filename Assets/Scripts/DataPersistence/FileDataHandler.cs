using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Tumbledown.DataPersistence {

	public class FileDataHandler
	{
		private string _dataFilePath = "";
		private string _dataFileName = "";

		private bool _useEncryption = false;
		private string _encryptionKey = "abracadabra"; // change this to a compile time var at some point

		public FileDataHandler(string dataFileName, bool encrypt = true)
		{
			_dataFileName = dataFileName;
			_dataFilePath = Path.Combine(Application.persistentDataPath, (_dataFileName + ".json") );
			_useEncryption = encrypt;
		}

		public T Load<T>()
		{
			T loadedData = default(T);

			if ( ! File.Exists(_dataFilePath))
			{
				Debug.Log("No data file found at " + _dataFilePath);
				return default(T);
			}

			try {
				// load data from file
				string dataToLoad = "";

				using (FileStream stream = new FileStream(_dataFilePath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}

				// decrypt if requested
				if (_useEncryption)
				{
					dataToLoad = EncryptDecrypt(dataToLoad);
				}

				// deserialize json
				loadedData = JsonUtility.FromJson<T>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.Log("Error loading data from file " + _dataFilePath + ": " + e.Message);
			}

			return loadedData;
		}

		public void Save(SaveData data)
		{
			try
			{
				// create dir if not exists
				Directory.CreateDirectory(Path.GetDirectoryName(_dataFilePath));

				// serialize to json
				string json = JsonUtility.ToJson(data, true);

				// encrypt if requested
				if (_useEncryption)
				{
					json = EncryptDecrypt(json);
				}

				// write data to file
				using (FileStream stream = new FileStream(_dataFilePath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(stream))
					{
						writer.Write(json);
					}
				}
			}
			catch (Exception e)
			{
				Debug.Log("Error saving data to file " + _dataFilePath + ": " + e.Message);
			}
		}

		// simple XOR encryption
		private string EncryptDecrypt(string data)
		{
			string modifiedData = "";
			for (int i = 0; i < data.Length; i++)
			{
				modifiedData += (char)(data[i] ^ _encryptionKey[i % _encryptionKey.Length]);
			}
			return modifiedData;
		}

		// delete the data file
		public void DeleteSaveFile()
		{
			if (File.Exists(_dataFilePath))
			{
				File.Delete(_dataFilePath);
			}
		}
	}
}