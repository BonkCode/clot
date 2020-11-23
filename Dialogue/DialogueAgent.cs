using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	[TextArea(3, 10)]
	public string[] sentences;
}

public class DialogueAgent : MonoBehaviour
{
	public Dialogue dialogue;

	public void Start()
	{
		TriggerDialogue();
	}

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}
}
