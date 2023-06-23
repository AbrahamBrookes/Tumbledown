using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.Collectables
{
	/**
	 * In our game, the player collects hidden feathers around each region. These feathers are
	 * unique to each region - plains region has emu feathers, forest region has owl feathers,
	 * etc. This class is responsible for saving the state of all the hidden feathers to the
	 * same save file.
	 */
    public class HiddenFeathersSaveData
    {
		// in order to track the state of each feather, each feather has a unique ID.
		// We then save a list of all feathers, indexed by the ID, and the state of each
		// feather is saved in the list.
		public Dictionary<string, HiddenFeatherSaveData> hiddenFeathers = new Dictionary<string, HiddenFeatherSaveData>();
	}
}
