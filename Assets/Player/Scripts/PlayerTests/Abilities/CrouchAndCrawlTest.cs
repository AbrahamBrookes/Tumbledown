using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

/**
 * Our player can crouch in order to hide behind low objects and in hollow bushes. They can then
 * move using the regular left stick input and crawl through small spaces.
 */
namespace Tumbledown.PlayerTests
{
    public class CrouchAndCrawlTest : InputTestFixture
    {
		[SetUp]
		public void SetUp(){
			SceneManager.LoadScene("Scenes/Testing/MovementTestScene");
		}

		/**
		 * When the player presses the crouch button, the protagonist crouches.
		 * When crouched, the animator should be in the Crouched state.
		 * When crouched, the capsule collider should be shorter.
		 * When the player releases the crouch button, the protagonist stands up.
		 * When standing, the animator should be in the Idle state.
		 * When standing, the capsule collider should be taller.
		 */
		[UnityTest]
		public IEnumerator PlayerCanCrouch()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists animator
			Animator animator = protagonist.GetComponent<Animator>();

			// do some input
			Set(gamepad.leftShoulder, 1);

			// wait for a tick
			yield return new WaitForSeconds(0.1f);
			
			// check that the protagonist is crouched
			// check that the animators isCrouching parameter is true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the protagonist's capsule collider is shorter
			Assert.AreEqual(0.1f, protagonist.GetComponent<CharacterController>().height);

			// release the input
			Set(gamepad.leftShoulder, 0);

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the protagonist is standing
			// check that the animators isCrouching parameter is false
			Assert.AreEqual(false, animator.GetBool("IsCrouching"));

			// check that the protagonist's capsule collider is taller
			Assert.AreEqual(0.8f, protagonist.GetComponent<CharacterController>().height);
		}

		/**
		 * When the player presses the crouch button, the protagonist crouches. Thereafter, the player
		 * can move the protagonist using the left stick input.
		 */
		[UnityTest]
		public IEnumerator PlayerCanCrawl()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists world location
			Vector3 protagonistWorldLocation = protagonist.transform.position;

			// cache the protagonists animator
			Animator animator = protagonist.GetComponent<Animator>();

			// do some input
			Set(gamepad.leftShoulder, 1);

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, 0f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// release the input
			Set(gamepad.leftShoulder, 0);

			// wait for a tick
			yield return new WaitForSeconds(0.1f);

			// check that the animators isCrawling parameter is false
			Assert.AreEqual(false, animator.GetBool("IsCrawling"));
		}

		/**
		 * The player can move all compass directions while crouched
		 */
		[UnityTest]
		public IEnumerator PlayerCanCrawlAllCompassDirections()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists world location
			Vector3 protagonistWorldLocation = protagonist.transform.position;

			// cache the protagonists animator
			Animator animator = protagonist.GetComponent<Animator>();

			// press the crouch button
			Set(gamepad.leftShoulder, 1);

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, 0f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing right
			Assert.AreEqual(90.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(0f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing up
			Assert.AreEqual(0.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, 0f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing left
			Assert.AreEqual(270.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(0f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing down
			Assert.AreEqual(180.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);
		}

		/**
		 * The player can move diagonally while crouched and they snap rotation to 45s
		 */
		[UnityTest]
		public IEnumerator PlayerCanCrawlDiagonally()
		{
			// create an input system
			var gamepad = InputSystem.AddDevice<Gamepad>();

			// instantiate our protagonist prefab in Assets/Player/prefabs/Kora
			var protagonist = Object.Instantiate(Resources.Load<GameObject>("Player/Kora"));

			// cache the protagonists world location
			Vector3 protagonistWorldLocation = protagonist.transform.position;

			// cache the protagonists animator
			Animator animator = protagonist.GetComponent<Animator>();

			// press the crouch button
			Set(gamepad.leftShoulder, 1);

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing right
			Assert.AreEqual(45.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(1f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing right
			Assert.AreEqual(135.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;
			
			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, -1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing left
			Assert.AreEqual(225.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);

			// cache the protagonists world location
			protagonistWorldLocation = protagonist.transform.position;

			// do some input
			Set(gamepad.leftStick, new Vector2(-1f, 1f));

			// wait for a tick
			yield return new WaitForSeconds(0.3f);

			// check that the protagonist has moved
			Assert.AreNotEqual(protagonistWorldLocation, protagonist.transform.position);

			// check that the animators isCrouching parameter is still true
			Assert.AreEqual(true, animator.GetBool("IsCrouching"));

			// check that the animators MovingSpeed parameter is greater than 0
			Assert.Greater(animator.GetFloat("MovingSpeed"), 0f);

			// the protagonists mesh should be facing left
			Assert.AreEqual(315.0f, protagonist.GetComponentInChildren<SkinnedMeshRenderer>().transform.rotation.eulerAngles.y);
		}
    }
}
