using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tumbledown.Input;

namespace Tumbledown {

	public class PlayerMovement : MonoBehaviour
	{
		private CharacterController _characterController;

		[SerializeField] private GameObject _mesh = default;
		
		private Animator _animator;
		
		// the input mapping is swapped out when we need to activate different controls
		[SerializeField] private InputMapping _inputMapping = default;

		public float moveSpeed = 5f;
		public float jumpForce = 5f;
		public float gravity = -9.8f;

		private Vector3 _moveDirection;
		private bool _isJumping;

		// different objects in the game world may apply a speed modifier to the players movement,
		// however we don't want to double up on the modifiers, and we don't want to have two of the
		// same gameobjects fighting over the modifier, so we'll use a list, indexed by strings,
		// containing a SpeedModifier struct that contains the modifier value and the tally of the 
		// number of objects applying that modifier. If the tally is zero, the modifier ceases to apply.
		private Dictionary<string, SpeedModifier> _speedModifiers = new Dictionary<string, SpeedModifier>();
		
		// our SpeedModifier struct
		public struct SpeedModifier
		{
			public float multiplier;
			public int tally;
		}

		// the speed multiplier is the product of all the speed modifiers in the list
		public float SpeedMultiplier
		{
			get
			{
				float multiplier = 1f;
				foreach (KeyValuePair<string, SpeedModifier> entry in _speedModifiers)
				{
					multiplier *= entry.Value.multiplier;
				}

				return multiplier;
			}
		}

		// a method to add a speed modifier to the list
		public void AddSpeedModifier(string key, float multiplier)
		{
			// if the key exists, increment the tally
			if (_speedModifiers.ContainsKey(key))
			{
				_speedModifiers[key] = new SpeedModifier
				{
					multiplier = multiplier,
					tally = _speedModifiers[key].tally + 1
				};
			}
			// otherwise add a new entry
			else
			{
				_speedModifiers.Add(key, new SpeedModifier
				{
					multiplier = multiplier,
					tally = 1
				});
			}
		}

		// a method to remove a speed modifier from the list
		public void RemoveSpeedModifier(string key)
		{
			// if the key exists, decrement the tally
			if (_speedModifiers.ContainsKey(key))
			{
				_speedModifiers[key] = new SpeedModifier
				{
					multiplier = _speedModifiers[key].multiplier,
					tally = _speedModifiers[key].tally - 1
				};

				// if the tally is zero, remove the entry
				if (_speedModifiers[key].tally == 0)
				{
					_speedModifiers.Remove(key);
				}
			}
		}

		private void Start()
		{
			// if no character controller, create one
			_characterController = GetComponent<CharacterController>();
			if (_characterController == null)
			{
				_characterController = gameObject.AddComponent<CharacterController>();
			}

			// if no animator, warn
			_animator = GetComponent<Animator>();
			if (_animator == null)
			{
				Debug.LogWarning("No animator found on game object using PlayerMovement");
			}
		}

		private void Awake() {
			_inputMapping = new GeneralGameplayInputMapping(this, _characterController);
			_inputMapping.ActivateMapping();
		}

		private void Update()
		{
			float horizontalInput = _inputMapping.MovementVector.x;
			float verticalInput = _inputMapping.MovementVector.y;

			_moveDirection = transform.forward * verticalInput + transform.right * horizontalInput * SpeedMultiplier;
			
			// Rotate
			if (_moveDirection.sqrMagnitude > 0f)
			{
				// rotate to face the input direction
				_mesh.transform.rotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
			}
			
			// set the walking speed to the current velocity
			_animator.SetFloat("MovingSpeed", Mathf.Clamp01(_characterController.velocity.magnitude));

			// Apply gravity
			_moveDirection.y += gravity;

			// Apply movement to CharacterController
			_characterController.Move(_moveDirection * moveSpeed * Time.deltaTime);
		}
		
		// allow other scripts to set the inputMapping
		public void SetInputMapping(Type inputMapping)
		{
			// disable the old input mapping if we have one
			if (_inputMapping != null)
				_inputMapping.DeactivateMapping();

			_inputMapping = (InputMapping)Activator.CreateInstance(inputMapping, this, _characterController);
			_inputMapping.ActivateMapping();
		}
	}
}