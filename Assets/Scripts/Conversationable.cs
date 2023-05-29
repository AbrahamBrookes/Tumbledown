using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * When the player interacts with a Conversationable, we show our conversation UI and start the
 * conversation. We animate the UI text to type into the UI, and allow the player to advance the
 * conversation by pressing their interact button. Input is handled in the Protagonist script.
 *
 * We add each conversation line to a queue, and then pop them off the queue as the player advances.
 * When we read the last line, the next interaction will close the conversation UI.
 *
 * Each conversation line coincides with other events, like camera matinees and and NPC animations.
 * For this reason, we'll also define a struct to hold the conversation line and any other events
 * that should happen at the same time.
 */

public class Conversationable : MonoBehaviour, Interactable
{
	// a reference to the .uxml unity UI document that defines our conversation UI
	[SerializeField] private GameObject _conversationUI = default;

	// a reference to the label that we will feed our text into, within that document
	[SerializeField] private UnityEngine.UIElements.Label _conversationLabel = default;

	// our ConversationLine struct
	public struct ConversationLine
	{
		public string text;
		public string animationTrigger;
		public string cameraTrigger;
	}

	public ConversationLine Line;

	// a queue of ConversationLines
	public Queue<ConversationLine> ConversationLines;

	// on start, warn if we don't have a reference to the conversation UI
	private void Start()
	{
		if (_conversationUI == null)
		{
			Debug.LogError("No conversation UI found - please assign one to the Conversationable script on " + gameObject.name);
		}

		if (_conversationLabel == null)
		{
			Debug.LogError("No conversation label found - please assign one to the Conversationable script on " + gameObject.name);
		}
	}

	// when the player interacts with us, start the conversation
	public void Interact()
	{
		Debug.Log("Interacting with " + gameObject.name);
		// Kick off our queue, which will handle each conversation line as the player advances
		StartCoroutine(ConversationQueue());
	}

	// this coroutine will handle each conversation line as the player advances
	private IEnumerator ConversationQueue()
	{
		// show the conversation UI
		ShowConversationUI();

		// while we have conversation lines in the queue
		while (ConversationLines.Count > 0)
		{
			// get the next line
			ConversationLine line = ConversationLines.Dequeue();

			// animate the text
			yield return StartCoroutine(AnimateText(line.text));

			// if we have an animation trigger, trigger it
			if (line.animationTrigger != "")
			{
				GetComponent<Animator>().SetTrigger(line.animationTrigger);
			}

			// if we have a camera trigger, trigger it
			// if (line.cameraTrigger != "")
			// {
			// 	GetComponent<CameraTriggerer>().TriggerCamera(line.cameraTrigger);
			// }

			// wait for the player to advance the conversation
			yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
		}

		// when we're done, hide the conversation UI
		HideConversationUI();
	}

	// this coroutine will animate the text
	private IEnumerator AnimateText(string text)
	{
		// clear the label
		_conversationLabel.text = "";

		// for each character in the text
		foreach (char c in text.ToCharArray())
		{
			// add the character to the label
			_conversationLabel.text += c;

			// wait a frame
			yield return null;
		}
	}

	// this method will show the conversation UI
	private void ShowConversationUI()
	{
		// enable/unhide the conversation UI
		_conversationUI.SetActive(true);
	}

	// this method will hide the conversation UI
	private void HideConversationUI()
	{
		// disable/hide the conversation UI
		_conversationUI.SetActive(false);
	}
}
