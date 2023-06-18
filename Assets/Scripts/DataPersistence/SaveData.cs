using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.DataPersistence
{
	/**
	 * SaveData is a very blank base class that you should extend in order to save your own data.
	 * When you want to save something, you need a class to serialize into a save file. That class
	 * should extend SaveData (for the sake of type safety). You can add whatever fields you want
	 * to persist, in your extending class.
	 */
    abstract public class SaveData {}
}
