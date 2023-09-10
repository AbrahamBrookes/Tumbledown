using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

/**
 * Our player can swing their sword to attack. The animator should flick over to attacking mode.
 */
namespace Tumbledown.PlayerTests
{
    public class AttackerTest : InputTestFixture
    {
		[SetUp]
		public void SetUp(){
			SceneManager.LoadScene("Scenes/Testing/MovementTestScene");
		}

		/**
		 * When the player presses the attack button, the animator shoudl play the attack animation
		 */
		[UnityTest]
		public IEnumerator PlayerCanAttack()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists animator
			Animator animator = protagonist.GetComponent<Animator>();

			// do some input
			Set(gamepad.buttonWest, 1);

			// wait for a tick
			yield return new WaitForSeconds(0.1f);
			
			// check that the animators IsAttacking parameter is true
			Assert.AreEqual(true, animator.GetBool("IsAttacking"));
			
			// wait for a sec
			yield return new WaitForSeconds(1f);
			
			// check that the animators IsAttacking parameter is false
			Assert.AreEqual(false, animator.GetBool("IsAttacking"));
		}
    }
}
