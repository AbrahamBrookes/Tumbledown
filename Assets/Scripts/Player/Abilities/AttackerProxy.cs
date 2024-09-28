using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.Abilities {

	/**
	 * The AttackerProxy passes all the attack calls from the Protagonist to the Attacker script that
	 * is attached to a child box collider which actually does the attacking.
	 */
	public class AttackerProxy : MonoBehaviour
	{
		// a reference to the attacker script
		[SerializeField] private Attacker _attacker;

		void Start()
		{
			// get the attacker script
			_attacker = GetComponentInChildren<Attacker>();	
		}
		
		// pass the FinishAttacking animation event to the attacker
		public void FinishAttacking()
		{
			_attacker.FinishAttacking();
		}

		// pass the ApplyDamage animation event to the attacker
		public void ApplyDamage()
		{
			_attacker.ApplyDamage();
		}
	}
}