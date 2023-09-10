using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tumbledown.Player;

namespace Tumbledown.Input {
	/**
	* This is the most common input mapping - walk around and hit stuff with your sword.
	*/

	public class CantDoAnythingInputMapping : InputMapping
	{
		// in the contructor pass the protagonist and the character controller directly to base
		public CantDoAnythingInputMapping(PlayerMovement movement, CharacterController characterController, Protagonist protagonist) : base(movement, characterController, protagonist) { }

		// these functions map directly to an action in the Input System
		public override void OnMove(InputAction.CallbackContext context) { }
		public override void OnAttack(InputAction.CallbackContext context) { }
		public override void OnInteract(InputAction.CallbackContext context) { }
		public override void OnPause(InputAction.CallbackContext context) { }
		public override void OnOpenInventory(InputAction.CallbackContext context) { }
	}
}
