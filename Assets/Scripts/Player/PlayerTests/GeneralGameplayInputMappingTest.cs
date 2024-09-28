using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

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
		[SetUp]
		public void SetUp(){
			SceneManager.LoadScene("Scenes/Testing/MovementTestScene");
		}

		/**
		* This test ensures that the OnMove function is working as expected:
		* - when the player presses the move button, the protagonist moves
		* - when the player releases the move button, the protagonist stops moving
		*/
		[UnityTest]
		public IEnumerator PlayerCanMoveCompassDirections()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists world location
			Vector3 protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, 0f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing right
			Assert.AreEqual(90.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(0f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing down
			Assert.AreEqual(0.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, 0f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing left
			Assert.AreEqual(270.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(0f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing up
			Assert.AreEqual(180.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);
		}

		/**
		 * The player can move in diagonals and the mesh rotates to face diagonally
		 */
		[UnityTest]
		public IEnumerator PlayerCanMoveDiagonally()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists world location
			Vector3 protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing right
			Assert.AreEqual(45.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing left
			Assert.AreEqual(315.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing left
			Assert.AreEqual(225.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// the protagonists mesh should be facing left
			Assert.AreEqual(135.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// release the input
			Set(gamepad.leftStick, new Vector2(0, 0));

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist has not moved, but ignore the y axis
			Assert.AreEqual(protagonistWorldLocation.x, protagonist.transform.position.x);
			Assert.AreEqual(protagonistWorldLocation.z, protagonist.transform.position.z);
		}
	}
}