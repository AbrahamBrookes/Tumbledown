using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/**
 * Our GeneralGameplayInputMapping controls the game when the player is walking around and hitting
 * stuff with their sword. This test ensures that the input mapping is working as expected.
 * 
 * The input mapping is using the Unity input system, so we can use their API to interact with our
 * input mapping scripts. Namely, passing an InputAction.CallbackContext into the input mapping's
 * On* functions.
 *
 * In this way, this test can test each logical path of all the public functions in the input
 * mapping script, for clean test coverage.
 */
namespace Tumbledown.PlayerTests
{
	public class GeneralGameplayInputMappingTest : InputTestFixture
	{
		/**
		* This test ensures that the OnMove function is working as expected:
		* - when the player presses the move button, the protagonist moves
		* - when the player releases the move button, the protagonist stops moving
		*/
		[UnityTest]
		public IEnumerator OnMoveTest()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// do some input
			Set(gamepad.leftStick, new Vector2(0.123f, 0.234f));

			// wait for 1 second
			yield return new WaitForSeconds(1);

			// check that the protagonist is moving
			Assert.IsTrue(protagonist.GetComponent<Rigidbody2D>().velocity.x != 0 || protagonist.GetComponent<Rigidbody2D>().velocity.y != 0);
		}
	}
}