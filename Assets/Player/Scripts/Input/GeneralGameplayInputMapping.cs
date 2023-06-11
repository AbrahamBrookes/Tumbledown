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
	public GeneralGameplayInputMapping(PlayerMovement movement, CharacterController characterController) : base(movement, characterController) {
		// on construct, cache deps
		_attacker = movement.GetComponent<Attacker>();
		_crouchAndCrawl = movement.GetComponent<CrouchAndCrawl>();
		_interactor = movement.GetComponent<Interactor>();
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
