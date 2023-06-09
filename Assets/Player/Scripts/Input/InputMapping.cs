using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tumbledown.Input {
/**
 * During gameplay the player interacts with the world and the things in it. These interactions
 * may cause the input scheme to change - for instance, when the player is looting a chest they
 * shouldn't be able to move until the chest looting animation is over, and the attack button
 * should be used to close the loot screen instead of attacking. When the player has picked up
 * a Liftable they can still run around but the attack and interact buttons should both cause
 * the player to throw the Liftable.
 *
 * To handle these different input modes, we'll swap out the InputMapping script on the player
 * to match the current input mode.
 *
 * This class extends the GameInput.IGameplayActions interface provided by the Unity Input System
 * so that we can use their fancy action map editor UI to define our input scheme. This requires
 * that you have created an action mapping set called 'Gameplay' since the GameInput interface is
 * generated from that action mapping UI.
 */

	abstract public class InputMapping : GameInput.IGameplayActions
	{
		// our game input
		protected GameInput _gameInput;

		// the current movement vector being input
		protected Vector2 _movementVector;

		// allow getting the movement vector
		public Vector2 MovementVector { get { return _movementVector; } }

		// we're always going to need a character controller
		protected CharacterController _characterController;

		// whenever we get input, update the movement vector
		public virtual void OnMove(InputAction.CallbackContext context) {
			// update our input vector
			_movementVector = context.ReadValue<Vector2>();
		}

		public virtual void OnAttack(InputAction.CallbackContext context) { }
		public virtual void OnInteract(InputAction.CallbackContext context) { }
		public virtual void OnPause(InputAction.CallbackContext context) { }
		public virtual void OnOpenInventory(InputAction.CallbackContext context) { }
		public virtual void OnCrouch(InputAction.CallbackContext context) { }

		// on construct, accept and cache our deps
		public InputMapping(PlayerMovement movement, CharacterController characterController)
		{
			_characterController = characterController;
		}

		// on activation, register this mapping with the InputManager
		public void ActivateMapping()
		{
			// if there is an existing mapping, deactivate it and remove it from input
			if (_gameInput == null)
			{
				_gameInput = new GameInput();
			}
			else
			{
				_gameInput.Gameplay.Disable();
				_gameInput.Gameplay.SetCallbacks(null);
			}

			_gameInput.Gameplay.SetCallbacks(this);
			_gameInput.Gameplay.Enable();
		}

		public void DeactivateMapping()
		{
			_gameInput.Gameplay.Disable();
			_gameInput.Gameplay.SetCallbacks(null);
		}
	}
}