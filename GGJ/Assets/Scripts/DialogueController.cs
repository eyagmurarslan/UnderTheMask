using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController ActiveDialogue; // 🔥 EN KRİTİK SATIR

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text dialogueText;

    [Header("Dialogue Manager")]
    public DialogueManager dialogueManager;

    [Header("Dialogue Lines")]
    [TextArea(2, 6)]
    public string[] dialogueLines;

    private int currentIndex = 0;

    public void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        ActiveDialogue = this; // 👈 BEN AKTİFİM
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentIndex];
    }

    public void NextDialogue()
    {
        if (ActiveDialogue != this)
            return;

        currentIndex++;

        if (currentIndex >= dialogueLines.Length)
        {
            dialoguePanel.SetActive(false);
            ActiveDialogue = null;
            dialogueManager.CloseAll();
            return;
        }

        dialogueText.text = dialogueLines[currentIndex];
    }
}