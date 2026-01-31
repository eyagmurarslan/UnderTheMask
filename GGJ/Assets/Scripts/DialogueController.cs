using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController ActiveDialogue; // mevcut satır

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text dialogueText;

    [Header("Dialogue Manager")]
    public DialogueManager dialogueManager;

    [Header("Dialogue Lines")]
    [TextArea(2, 6)]
    public string[] dialogueLines;

    [Tooltip("Bu karakter/etkileşim için benzersiz ID (ör: 'char_01'). Dialogue tamamlandığında bu ID GameProgressTracker'a gönderilecek.")]
    public string characterID;

    private int currentIndex = 0;

    void Start()
    {
        // Kendini ilerleme sistemine kaydet
        if (!string.IsNullOrEmpty(characterID) && GameProgressTracker.Instance != null)
        {
            GameProgressTracker.Instance.RegisterTarget(characterID);
        }
    }

    public void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        ActiveDialogue = this;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentIndex];

        if (dialogueManager != null)
            dialogueManager.ToggleDialogue(dialoguePanel);
    }

    public void NextDialogue()
    {
        if (ActiveDialogue != this)
            return;

        currentIndex++;

        if (currentIndex >= dialogueLines.Length)
        {
            // Diyalog bitti: panel kapat ve progress'e bildir
            dialoguePanel.SetActive(false);
            ActiveDialogue = null;

            if (!string.IsNullOrEmpty(characterID) && GameProgressTracker.Instance != null)
            {
                GameProgressTracker.Instance.MarkCompleted(characterID);
            }

            if (dialogueManager != null)
                dialogueManager.CloseAll();

            return;
        }

        dialogueText.text = dialogueLines[currentIndex];
    }
}