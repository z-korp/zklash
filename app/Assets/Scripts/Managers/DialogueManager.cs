using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject dialogueBubble;
    public Text dialogueText;
    public Transform npc;
    public float displayDuration = 2f; // Time of display for message

    void Update()
    {
        if (npc != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(npc.position); // Follow NPC
            dialogueBubble.transform.position = screenPosition + new Vector3(0, 140, 0); // Offset
        }
    }

    public void ShowDialogue(string message)
    {
        dialogueText.text = message;
        dialogueBubble.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueBubble.SetActive(false);
    }

    public void ShowDialogueForDuration(string message, float duration)
    {
        StartCoroutine(ShowAndHideDialogue(message, duration));
    }

    private IEnumerator ShowAndHideDialogue(string message, float duration)
    {
        ShowDialogue(message);
        yield return new WaitForSeconds(duration);
        HideDialogue();
    }
}
