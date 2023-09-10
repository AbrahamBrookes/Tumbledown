using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
	private Label dialogueText;
	private Conversation _currentConversation;
	private int _currentLineIndex;

	// singleton
	public static DialogueManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		dialogueText = GetComponent<UIDocument>().rootVisualElement.Q<Label>("DialogueText");
	}

	public static void StartConversation(Conversation conversation)
	{
		Instance._currentLineIndex = 0;
		Instance._currentConversation = conversation;
		Instance.dialogueText.text = conversation.Lines[Instance._currentLineIndex].text;
	}

	public static void AdvanceConversation()
	{
		Instance._currentLineIndex++;
		if (Instance._currentLineIndex < Instance._currentConversation.Lines.Length)
		{
			Instance.dialogueText.text = Instance._currentConversation.Lines[Instance._currentLineIndex].text;
		}
		else
		{
			// TODO close the dialogue box
		}
	}
}
