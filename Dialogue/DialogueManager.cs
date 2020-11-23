using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Text dialogueText;
	public GameObject dialogueBox;
	public string lastMessage = null;
    public Queue<string> sentences;
	public float startTimer = 0f;
	public bool dialogueStarted = false;

	private void Start()
	{
		sentences = new Queue<string>();
	}

	public void Update()
	{
		if ((Input.GetKey(KeyCode.W) ||
			Input.GetKey(KeyCode.A) ||
			Input.GetKey(KeyCode.S) ||
			Input.GetKey(KeyCode.D)) &&
			!dialogueStarted)
		{
			DisplayNextSentence();
			dialogueStarted = true;
		}
		else if (!dialogueStarted)
		{
			if (startTimer <= 0)
			{
				DisplaySameSentence();
				startTimer = Random.Range(2f, 5f);
			}
			else
				startTimer -= Time.deltaTime;
		}
	}

	public void StartDialogue(Dialogue _dialogue)
	{
		Debug.Log("AAA");
		sentences.Clear();

		foreach (string _sentence in _dialogue.sentences)
		{
			sentences.Enqueue(_sentence);
		}
	}

	public void DisplaySameSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return ;
		}

		if (lastMessage == string.Empty)
			lastMessage = sentences.Dequeue();
		dialogueText.text = lastMessage;
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return ;
		}

		string sentence = sentences.Dequeue();
		dialogueText.text = sentence;
	}
	
	private void EndDialogue()
	{
		dialogueBox.SetActive(false);
	}
}