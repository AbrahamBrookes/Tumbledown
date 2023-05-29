using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The interactable interface provides a contract for items that can be interacted with. This is
 * extended by any items that need to be interacted with - lifting rocks, talking to npc's, etc.
 */

public interface Interactable
{
	// the method that will be called when the player interacts with the object
	void Interact();

	// an interactable always has a transform
	Transform transform { get; }

	// an interactable always has a game object
	GameObject gameObject { get; }
}
