using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown.Abilities;

namespace Tumbledown {

	public class MySlashable : MonoBehaviour, iSlashable
	{
		public void BeSlashed(Attacker attacker)
		{
			Debug.Log("I've been slashed!");
		}
	}

}