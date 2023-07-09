using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.Abilities {
	
	/**
	* This script is attached to the protagonist and handles crouching and crawling.
	*
	* These functions will be called via the Protagonist script since that is where the input is handled.
	*/

	public class CrouchAndCrawl : MonoBehaviour
	{
		// a reference to the protagonist, for updating movement speed
		[SerializeField] private Protagonist _protagonist = default;

		// are we crouching?
		private bool _isCrouching = false;

		// a speed multiplier for slowing down when crouching
		[SerializeField] private float _crawlSpeedMultiplier = 0.6f;

		// for accessing the capsule collider to make it shorter when crouching
		private CharacterController _characterController;
		
		// our animator component
		private Animator _animator;

		// used to set the size of the character controllers capsule when crawling and resetting
		[SerializeField] private float _crawlHeight = 0.1f;
		[SerializeField] private Vector3 _crawlcenter = new Vector3(0f, 0.2f, 0f);
		[SerializeField] private float _walkHeight = 0.8f;
		[SerializeField] private Vector3 _walkcenter = new Vector3(0f, 0.4f, 0f);

		// cache stuff start
		private void Awake()
		{
			_protagonist = GetComponent<Protagonist>();
			_characterController = GetComponent<CharacterController>();
			_animator = GetComponent<Animator>();
		}

		
		public void Crouch()
		{
			_isCrouching = true;
			_animator.SetBool("IsCrouching", _isCrouching);

			_characterController.height = _crawlHeight;
			_characterController.center = _crawlcenter;

			// apply our movement speed multiplier
			_protagonist.ApplySpeedMultiplier(_crawlSpeedMultiplier);
		}

		public void StandUp()
		{
			_isCrouching = false;
			_animator.SetBool("IsCrouching", _isCrouching);

			// reset the capsule collider
			_characterController.height = _walkHeight;
			_characterController.center = _walkcenter;

			// remove our movement speed multiplier
			_protagonist.RemoveSpeedMultiplier(_crawlSpeedMultiplier);
		}
	}
}