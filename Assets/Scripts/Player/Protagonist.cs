using System;
using UnityEngine;
using Tumbledown.Input;

namespace Tumbledown.Player {

	public class Protagonist : MonoBehaviour
	{
		// the input mapping is swapped out when we need to activate different controls
		[SerializeField] private InputMapping _inputMapping = default;
		[SerializeField] private Transform _gameplayCameraTransform = default;

		private Vector2 _inputVector;
		private float _previousSpeed;

		//These fields are read and manipulate
		[NonSerialized] public bool extraActionInput;
		[NonSerialized] public Vector3 movementInput; // Initial input
		[NonSerialized] public Vector3 movementVector; // Final movement vector
		[NonSerialized] public ControllerColliderHit lastHit;

		// movement speeds
		[SerializeField] private float _runSpeed = 1.4f;
		// allow getting runspeed
		public float RunSpeed => _runSpeed;
		[SerializeField] private float _acceleration = 5f;

		public const float GRAVITY_MULTIPLIER = 50f;
		public const float MAX_FALL_SPEED = -50f;
		public const float MAX_RISE_SPEED = 100f;
		public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
		public const float GRAVITY_DIVIDER = .6f;
		public const float AIR_RESISTANCE = 5f;

		private CharacterController _characterController;
		
		// our animator component
		private Animator _animator;

		// cache some stuff on awake
		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			lastHit = hit;
		}

		// allow other script to apply multipliers to movement speed
		public void ApplySpeedMultiplier(float multiplier)
		{
			_runSpeed *= multiplier;
		}

		// remove a speed multiplier
		public void RemoveSpeedMultiplier(float multiplier)
		{
			_runSpeed /= multiplier;
		}

		// proxy SetInputMapping to out PlayerMovement component
		public void SetInputMapping(Type inputMapping)
		{
			GetComponent<PlayerMovement>().SetInputMapping(inputMapping);
		}

		// proxy ReturnToPreviousInputMapping to our PlayerMovement component
		public void ReturnToPreviousInputMapping()
		{
			GetComponent<PlayerMovement>().ReturnToPreviousInputMapping();
		}
	}
}