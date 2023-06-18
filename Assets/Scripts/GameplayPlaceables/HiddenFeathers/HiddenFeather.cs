using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown.DataPersistence;

namespace Tumbledown.Collectables
{
	/**
	 * In our game, the player collects hidden feathers around each region. These feathers are
	 * unique to each region - plains region has emu feathers, forest region has owl feathers,
	 * etc.
	 */
    public class HiddenFeather : MonoBehaviour
    {
		// a HiddenFeather will save when it is picked up, so we must always have a Saveable
		private Saveable _saveable;

		// and this is the save state of this feather
		private HiddenFeatherSaveData _saveData;

		// the ID of this feather. This is unique to each feather in the game. Manually set
		// this when you place the feather in the scene.
		public string featherID;

        // Start is called before the first frame update
        void Start()
        {
			// make sure we have a Saveable
			_saveable = GetComponent<Saveable>();
			if (_saveable == null)
			{
				Debug.LogError("HiddenFeather does not have a Saveable component attached!");

				// disable this component
				enabled = false;

				return;
			}

			// make sure we have a feather ID
			if (featherID == null || featherID == "")
			{
				Debug.LogError("HiddenFeather does not have a feather ID set!");

				// disable this component
				enabled = false;

				return;
			}

			// load the state of the feather
			Load();
        }

		// When the player collects this feather, we want to show a UI popup and then save the
		// state of the feather to the save file.
		public void Collect()
		{
			// show a UI popup

			// save the state of the feather
			_saveData.isCollected = true;

			// save it
			Save();
		}

		// Save the state of the feather to the save file
		public void Save()
		{
			// call the save method on our Saveable
			HiddenFeathersSaveData library = _saveable.Load<HiddenFeathersSaveData>();

			// update the save data for this feather
			library.hiddenFeathers[featherID] = _saveData;

			// save the library back to the save file
			_saveable.Save(library);
		}

		// Load the state of the feather from the save file
		public void Load()
		{
			// call the load method on our Saveable
			HiddenFeathersSaveData library = _saveable.Load<HiddenFeathersSaveData>();

			// find the feather in the save data
			if (library.hiddenFeathers.ContainsKey(featherID))
			{
				// get the state of the feather
				_saveData = library.hiddenFeathers[featherID];
			}
			else
			{
				// feather is not in the save data, this is a worry
				Debug.LogError("HiddenFeather " + featherID + " is not in the save data!");

				// disable this component
				enabled = false;
			}
		}
    }
}
