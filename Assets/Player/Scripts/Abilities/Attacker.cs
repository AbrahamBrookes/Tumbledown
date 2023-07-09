using UnityEngine;
using System.Collections.Generic;
using Tumbledown.Input;

namespace Tumbledown.Abilities {

	/**
	* Our attacker script relies on the interactor collider - when a gameobject containing a 
	* component that implementes the iSlashable interface enters the collider, we add it to
	* a list of slashable objects. When the player attacks, we iterate through the list and
	* call the slash method on each slashable object, passing in the direction of the attack.
	*/

	public class Attacker : MonoBehaviour
	{
		// the attack collider
		[SerializeField] private GameObject _attackCollider;

		// we need a reference to the animator so we can make animation calls
		[SerializeField] private Animator _animator;

		// a reference to the protagonist script, so we can disable movement while attacking
		[SerializeField] private Protagonist _protagonist;

		// our list of slashables
		private List<iSlashable> _slashableObjects = new List<iSlashable>();

		// on start, cache deps
		private void Start()
		{
			_animator = GetComponentInParent<Animator>();
			_protagonist = GetComponentInParent<Protagonist>();

			// if we don't have an attack collider already set, log an error
			if (_attackCollider == null)
			{
				Debug.LogError("No attack collider set on " + gameObject.name);
			}
		}

		/**
		* Whenever a gameobject with a component that implements the iSlashable interface
		* enters the attack collider, we add it to a list of slashable objects.
		*/
		private void OnTriggerEnter(Collider other)
		{
			// if the other gameobject has a component that implements the iSlashable interface
			if (other.GetComponent<iSlashable>() != null)
			{
				// add it to the list of slashable objects
				_slashableObjects.Add(other.GetComponent<iSlashable>());
			}
		}

		/**
		* Whenever a gameobject with a component that implements the iSlashable interface
		* leaves the attack collider, we remove it from the list of slashable objects.
		*/
		private void OnTriggerExit(Collider other)
		{
			// if the other gameobject exists in our list of slashable objects
			if (_slashableObjects.Contains(other.GetComponent<iSlashable>()))
			{
				// remove it from the list
				_slashableObjects.Remove(other.GetComponent<iSlashable>());
			}
		}

		// when the player presses the attack button, start the animation
		public void StartAttack()
		{
			// disable input while attacking
			_protagonist.SetInputMapping(typeof(CurrentlyAttackingInputMapping));

			// play the attack animation
			_animator.SetBool("IsAttacking", true);
			_animator.Play("Attack", 0, 0f);
		}

		// called from an animation event - actually apply damage to the slashable objects
		public void ApplyDamage()
		{
			// if we have slashable objects in our list
			if (_slashableObjects.Count > 0)
			{
				// iterate through the list
				foreach (iSlashable slashableObject in _slashableObjects)
				{
					// call the slash method on each slashable object, passing in the direction of the attack
					slashableObject.BeSlashed(this);
				}
			}
		}

		// called from an animation event, or when attacking is interrupted
		public void FinishAttacking()
		{
			// flick back to the general gameplay input mapping
			_protagonist.SetInputMapping(typeof(GeneralGameplayInputMapping));

			_animator.SetBool("IsAttacking", false);
		}
	}
}