using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/New Conversation")]
public class Conversation : ScriptableObject
{
	[SerializeField] private DialogueLine[] _lines;

	public DialogueLine[] Lines => _lines;
}