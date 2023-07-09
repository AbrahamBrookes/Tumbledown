using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tumbledown.Abilities;

namespace Tumbledown.Input {
	/**
	* The input mapping for when we are currently attacking - stop all movement but allow more attacks.
	*/

	public class CurrentlyAttackingInputMapping : InputMapping
	{
		// a reference to the protagonists attack script
		Attacker _attacker;

		// in the contructor pass the protagonist and the character controller directly to base
		public CurrentlyAttackingInputMapping(PlayerMovement movement, CharacterController characterController) : base(movement, characterController) {
			// on construct, cache the attacker script
			_attacker = movement.GetComponent<Attacker>();
		}

		// don't allow movement while attacking
		public override void OnMove(InputAction.CallbackContext context) { }

		public override void OnAttack(InputAction.CallbackContext context) {
			// if the button has been pressed
			if (context.performed) {
				_attacker.StartAttack();
			}
		}

		public override void OnInteract(InputAction.CallbackContext context) { }
		public override void OnPause(InputAction.CallbackContext context) { }
		public override void OnOpenInventory(InputAction.CallbackContext context) { }
	}
}