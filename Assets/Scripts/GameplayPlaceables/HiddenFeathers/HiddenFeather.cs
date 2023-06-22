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
		// a HiddenFeather will save when it is picked up, so we must always have a SaveFile
		private SaveFile _savefile = new SaveFile("HiddenFeathers");

		// and this is the save state of this feather
		private HiddenFeatherSaveData _saveData = new HiddenFeatherSaveData();

		// the ID of this feather. This is unique to each feather in the game. Manually set
		// this when you place the feather in the scene.
		public string FeatherID;

        // Start is called before the first frame update
        void Start()
        {
			// make sure we have a feather ID
			if (FeatherID == null || FeatherID == "")
			{
				Debug.LogError("HiddenFeather does not have a feather ID set!");

				// disable this component
				enabled = false;

				return;
			}
		}

		// on Awake, load up
		private void Awake() {
			// load the state of the feather
			Load();
        }

		// When the player collects this feather, we want to show a UI popup and then save the
		// state of the feather to the save file.
		public void Collect()
		{
			// show a UI popup
			Debug.Log("You collected a feather!");

			// save the state of the feather
			_saveData.isCollected = true;

			// save it
			Save();
		}

		// Save the state of the feather to the save file
		public void Save()
		{
			// call the save method on our SaveFile
			HiddenFeathersSaveData library = _savefile.Load<HiddenFeathersSaveData>();

			// if we didn't get a library, create one (maybe the game hasn't been saved yet)
			if (library == null)
			{
				library = new HiddenFeathersSaveData();
			}

			// update the save data for this feather
			library.hiddenFeathers[FeatherID] = _saveData;
			
			// save the library back to the save file
			_savefile.Save(library);
		}

		// Load the state of the feather from the save file
		public void Load()
		{
			// call the load method on our SaveFile
			HiddenFeathersSaveData library = _savefile.Load<HiddenFeathersSaveData>();

			// check we have a library
			if (library == null)
			{
				Debug.LogError("HiddenFeather could not load the HiddenFeathersSaveData!");
				return;
			}

			// find the feather in the save data
			if (! library.hiddenFeathers.ContainsKey(FeatherID))
			{
				// feather is not in the save data, this is a worry
				Debug.LogError("HiddenFeather " + FeatherID + " is not in the save data!");
				return;
			}

			// get the state of the feather
			_saveData = library.hiddenFeathers[FeatherID];

			// if isCollected is true, then the player has already collected this feather
			// move this gameobject up 5 units so it is out of the way
			if (_saveData.isCollected)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
			}
		}
    }
}
