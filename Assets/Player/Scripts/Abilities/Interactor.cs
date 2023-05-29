using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is attached to the Protagonist and handles interactions with objects in the world.
 *
 * We have a box collider which we detect entries to, and then we check if the object we've collided
 * with has the Interactable interface. If it does, we add it to our list of interactables.
 *
 * When it leaves the collider, we remove the object from our list of interactables.
 * 
 * Each frame, if we have interactables in our list, find the closest one and call it our current
 * interactable. If the player presses the interact button, we call the Interact() method on the
 * current interactable.
 */

public class Interactor : MonoBehaviour
{
	// our collider
	[SerializeField] private BoxCollider _collider;

	// a list of interactables that we're currently colliding with
	private List<Interactable> _interactables;

	// the current interactable
	private Interactable _currentInteractable;

	// when something enters our collider, check if it's an interactable and add it to our list
	private void OnTriggerEnter(Collider other)
	{
		// see if the other object has a component that implements the Interactable interface
		Interactable interactable = other.GetComponent<Interactable>();
		Debug.Log("Interactable: " + interactable);

		if (interactable != null)
		{
			_interactables.Add(interactable);
		}
	}

	// when something leaves our collider, check if it's an interactable and remove it from our list
	private void OnTriggerExit(Collider other)
	{
		Interactable interactable = other.GetComponent<Interactable>();

		if (interactable != null)
		{
			_interactables.Remove(interactable);
		}
	}

	// on start, validate our collider and initialize our list
	private void Start()
	{
		if( _collider == null )
		{
			Debug.LogError("No interaction collider found - please assign one to the Interactor script on " + gameObject.name);
		}

		_interactables = new List<Interactable>();
	}

	// on update, if we have interactables in our list, find the closest one and call it our current interactable
	private void Update()
	{
		if (_interactables.Count > 0)
		{
			_currentInteractable = FindClosestInteractable();
		}
	}

	// find the closest interactable in our list
	private Interactable FindClosestInteractable()
	{
		Interactable closestInteractable = null;
		float closestDistance = Mathf.Infinity;
		Vector3 position = transform.position;

		foreach (Interactable interactable in _interactables)
		{
			Vector3 direction = interactable.transform.position - position;
			float distance = direction.sqrMagnitude;

			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestInteractable = interactable;
			}
		}

		return closestInteractable;
	}
	
	// if the player presses the interact button, call the Interact() method on the current interactable
	public void Interact()
	{
		// debug log the list of interactables
		string interactables = "Interactables: ";
		foreach (Interactable interactable in _interactables)
		{
			interactables += interactable.gameObject.name + ", ";
		}
		Debug.Log(interactables);
		
		if (_currentInteractable != null)
		{
			_currentInteractable.Interact();
		}
	}
}
