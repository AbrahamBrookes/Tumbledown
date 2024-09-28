using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown;
using Tumbledown.Abilities;

public class SlashyGrass : MonoBehaviour, iSlashable
{
	private bool isPlayerInTrigger = false;

	// we'll be using the PlayerMovement.SpeedMultiplier struct, passing in this float as the multiplier
	private float _speedModifier = 0.5f;

	/**
	 * When the player enters our trigger volumen, multiply their movement speed by 0.5
	 */
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerMovement>().AddSpeedModifier("SlashyGrass", _speedModifier);
			isPlayerInTrigger = true;
		}
	}

	/**
	 * When the player exits our trigger volume, reset their movement speed to 1
	 */
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerMovement>().RemoveSpeedModifier("SlashyGrass");
			isPlayerInTrigger = false;
		}
	}

	/**
	 * When the player slashes us, destroy us
	 */
	public void BeSlashed(Attacker attacker)
	{
		if (isPlayerInTrigger)
		{
			attacker.GetComponentInParent<PlayerMovement>().RemoveSpeedModifier("SlashyGrass");
		}

		// disable self
		gameObject.SetActive(false);
	}
}
