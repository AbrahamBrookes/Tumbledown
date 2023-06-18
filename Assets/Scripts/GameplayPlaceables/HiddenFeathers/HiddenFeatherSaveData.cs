using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown.DataPersistence;

namespace Tumbledown.Collectables
{
	/**
	 * In our game, the player collects hidden feathers around each region. These feathers are
	 * unique to each region - plains region has emu feathers, forest region has owl feathers,
	 * etc. This class is the save state of any single feather
	 */
    public class HiddenFeatherSaveData : SaveData
    {
		public bool isCollected;
	}
}
