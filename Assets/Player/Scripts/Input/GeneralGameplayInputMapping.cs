using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This is the most common input mapping - walk around and hit stuff with your sword.
 */

public class GeneralGameplayInputMapping : InputMapping
{
	// a reference to the protagonists attack script
	Attacker _attacker;

	// in the contructor pass the protagonist and the character controller directly to base
	public GeneralGameplayInputMapping(Protagonist protagonist, CharacterController characterController) : base(protagonist, characterController) {
		// on construct, cache the attacker script
		_attacker = _protagonist.GetComponent<Attacker>();
	}

	// these functions map directly to an action in the Input System
	public override void OnMove(InputAction.CallbackContext context) {
		// pass the input vector to the protagonist and let them move aboot
		_protagonist.UpdateInput(context.ReadValue<Vector2>());
	}

	public override void OnAttack(InputAction.CallbackContext context) {
		// if the button has been pressed
		if (context.performed) {
			_attacker.Attack();
		}
	}

	public override void OnInteract(InputAction.CallbackContext context) {
		// if the button has been pressed
		if (context.performed) {
			// get the interaction manager attached to the protagonist
			// InteractionManager interactionManager = _protagonist.GetComponent<InteractionManager>();
			// // if there is an interaction manager
			// if (interactionManager != null) {
			// 	// tell it to interact
			// 	interactionManager.OnInteractionButtonPress();
			// }
		}
	}
	public override void OnPause(InputAction.CallbackContext context) { }
	public override void OnOpenInventory(InputAction.CallbackContext context) { }

}
