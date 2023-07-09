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

			_moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
			
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