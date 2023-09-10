using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown.Player;

/**
 * The interactable interface provides a contract for items that can be interacted with. This is
 * extended by any items that need to be interacted with - lifting rocks, talking to npc's, etc.
 * 
 * @method Interact() The method that will be called when the player interacts with the object
 * @property transform The transform of the object
 * @property gameObject The game object of the object
 */

namespace Tumbledown {
	public interface Interactable {
		void Interact(Protagonist protagonist);
		Transform transform { get; }
		GameObject gameObject { get; }
	}
}