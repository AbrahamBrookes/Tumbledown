using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private UIDocument _conversationUI;
	private Label dialogueText;
	private Conversation _currentConversation;
	private int _currentLineIndex;
	
    [SerializeField] private float _fontRatio = 10;

	// public getter to allow other scripts to query whether we're in a conversation
	public static bool InConversation => Instance._currentConversation != null;

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

		dialogueText = _conversationUI.rootVisualElement.Q<Label>("DialogueText");

		// hide the conversation UI
		ToggleConversationUI(false);
	}

	void OnGUI()
	{
		// dynamically scale the font to match the screen size
        float finalSize = (float)Screen.width/_fontRatio;
        dialogueText.style.fontSize = (int)finalSize;
		StyleLength pixelOffsetX = new StyleLength(Screen.width/_fontRatio);
		StyleLength pixelOffsetY = new StyleLength(Screen.height/_fontRatio);
        dialogueText.style.left = pixelOffsetX;
        dialogueText.style.right = pixelOffsetX;
        dialogueText.style.top = pixelOffsetY;
        dialogueText.style.bottom = pixelOffsetY;

    }

	public static void StartConversation(Conversation conversation)
	{
		// show the conversation UI
		Instance.ToggleConversationUI(true);

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
			// if we have no lines left, throw an error
			throw new Tumbledown.Exceptions.NoLinesLeftInConversationException("No lines left in conversation");
		}
	}

	public static void EndConversation()
	{
		// null out current conversation
		Instance._currentConversation = null;

		// hide the conversation UI
		Instance.ToggleConversationUI(false);
	}

	private void ToggleConversationUI(bool show)
	{
		_conversationUI.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
	}
}
