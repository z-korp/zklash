using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public GameObject dialogueBubble;
    public Text dialogueText;
    public Transform npc;
    public float displayDuration = 2f; // Time of display for message

    private void Awake()
    {
        // Vérifiez s'il y a déjà une instance de DialogueManager
        if (Instance != null && Instance != this)
        {
            Debug.LogError("ERROR: only one instance of DialogueManager should be there destroying previous one");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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
