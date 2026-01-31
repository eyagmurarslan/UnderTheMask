using UnityEngine;

public class CharacterClick : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject dialoguePanel;

    public void OnButtonClicked()
    {
        dialogueManager.ToggleDialogue(dialoguePanel);
    }
}