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

	// a reference to the protagonists CrouchAndCrawl script
	CrouchAndCrawl _crouchAndCrawl;

	// a reference to the protagonists Interactor script
	Interactor _interactor;

	// in the contructor pass the protagonist and the character controller directly to base
	public GeneralGameplayInputMapping(Protagonist protagonist, CharacterController characterController) : base(protagonist, characterController) {
		// on construct, cache deps
		_attacker = _protagonist.GetComponent<Attacker>();
		_crouchAndCrawl = _protagonist.GetComponent<CrouchAndCrawl>();
		_interactor = _protagonist.GetComponent<Interactor>();
	}

	// these functions map directly to an action in the Input System
	public override void OnMove(InputAction.CallbackContext context) {
		// only if performed
		if (context.performed) {
			// pass the input vector to the protagonist and let them move aboot
			_protagonist.UpdateInput(context.ReadValue<Vector2>());
		}

		// if the button has been released, stop moving
		if (context.canceled) {
			_protagonist.UpdateInput(Vector2.zero);
		}
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
			_interactor.Interact();
		}
	}
	public override void OnPause(InputAction.CallbackContext context) { }
	public override void OnOpenInventory(InputAction.CallbackContext context) { }
	public override void OnCrouch(InputAction.CallbackContext context) {
		// if the button has been pressed, crouch
		if (context.performed) _crouchAndCrawl.Crouch();
		// if the button has been released, stand up
		if (context.canceled) _crouchAndCrawl.StandUp();
	}

}
