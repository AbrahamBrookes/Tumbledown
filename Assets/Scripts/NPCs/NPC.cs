using UnityEngine;
using Tumbledown.Player;
using Tumbledown.Input;

namespace Tumbledown {
	public class NPC : MonoBehaviour, Interactable
	{
		[SerializeField] private Conversation _conversation;

		public void Interact(Protagonist protagonist)
		{

			// if the DialogueManager is not in a conversation, start one
			if(!DialogueManager.InConversation)
			{
				// flick the input mapping over to dialogue
				protagonist.SetInputMapping(typeof(DialogueInputMapping));

				// start the conversation
				DialogueManager.StartConversation(_conversation);
			}
			// otherwise, advance the conversation
			else
			{
				try {
					DialogueManager.AdvanceConversation();
				}
				catch (Tumbledown.Exceptions.NoLinesLeftInConversationException e)
				{
					// if we have no lines left, end the conversation
					DialogueManager.EndConversation();
					
					// and flick the input mapping back to the default
					protagonist.ReturnToPreviousInputMapping();
				}
			}
		}
	}
}