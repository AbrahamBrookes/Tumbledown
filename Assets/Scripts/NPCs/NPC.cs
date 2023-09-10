using UnityEngine;
using Tumbledown.Player;
using Tumbledown.Input;

namespace Tumbledown {
	public class NPC : MonoBehaviour, Interactable
	{
		[SerializeField] private Conversation _conversation;

		public void Interact(Protagonist protagonist)
		{
			// flick the input mapping over to dialogue
			protagonist.SetInputMapping(typeof(DialogueInputMapping));

			DialogueManager.StartConversation(_conversation);
		}
	}
}