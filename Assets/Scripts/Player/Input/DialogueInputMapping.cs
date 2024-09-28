using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tumbledown.Abilities;
using Tumbledown.Player;

namespace Tumbledown.Input {
	/**
	* When we are in a dialogue we can't walk around or do anything except talk to the NPC.
	*/

	public class DialogueInputMapping : InputMapping
	{
		// a reference to the protagonists Interactor script
		Interactor _interactor;

		// in the contructor pass the protagonist and the character controller directly to base
		public DialogueInputMapping(PlayerMovement movement, CharacterController characterController, Protagonist protagonist) : base(movement, characterController, protagonist) {
			// on construct, cache deps
			_interactor = protagonist.GetComponentInChildren<Interactor>();
		}

		public override void OnInteract(InputAction.CallbackContext context) {
			// if the button has been pressed
			if (context.performed) {
				_interactor.Interact();
			}
		}

		public override void OnMove(InputAction.CallbackContext context) { }
		public override void OnAttack(InputAction.CallbackContext context) { }
		public override void OnPause(InputAction.CallbackContext context) { }
		public override void OnOpenInventory(InputAction.CallbackContext context) { }
	}
}
