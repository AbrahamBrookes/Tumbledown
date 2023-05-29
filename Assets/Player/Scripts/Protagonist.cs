using System;
using UnityEngine;

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

	[SerializeField] private GameObject mesh = default;

	// cache some stuff on awake
	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
		_inputMapping = new GeneralGameplayInputMapping(this, _characterController);
		_inputMapping.ActivateMapping();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	private void Update()
	{
		RecalculateMovement();
	}

	private void RecalculateMovement()
	{
		float targetSpeed;

		movementInput = new Vector3(_inputVector.x, 0f, _inputVector.y);

		// Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		if (targetSpeed > 0f)
		{
			targetSpeed = _runSpeed;
		}
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * _acceleration);
		
		// Rotate
		if (targetSpeed > 0f)
		{
			if (_inputVector.sqrMagnitude > 0f) {
				// rotate to face the input direction
				_mesh.transform.rotation = Quaternion.LookRotation(movementInput, Vector3.up);
			}
		}

		movementVector.x = movementInput.normalized.x * targetSpeed;
		movementVector.z = movementInput.normalized.z * targetSpeed;
		// fall to the ground
		movementVector.y = Mathf.Max(movementVector.y + Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime, MAX_FALL_SPEED);

		_characterController.Move(movementVector * Time.deltaTime);

		// set the walking speed to the current velocity
		_animator.SetFloat("MovingSpeed", Mathf.Clamp01(_characterController.velocity.magnitude));

		_previousSpeed = targetSpeed;

		// UpdateCamera();
	}

	

	//---- EVENT LISTENERS ----

	public void UpdateInput(Vector2 movement)
	{
		_inputVector = movement;
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
