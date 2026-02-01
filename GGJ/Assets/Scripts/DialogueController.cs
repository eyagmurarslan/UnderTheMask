using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController ActiveDialogue;

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text speakerText; // 🔹 KİM KONUŞUYOR TEXT

    [Header("Speaker Names")]
    public string playerName = "Oyuncu";
    public string characterName = "NPC";

    [Header("Dialogue Manager")]
    public DialogueManager dialogueManager;

    [Header("Dialogue Lines")]
    [TextArea(2, 6)]
    public string[] dialogueLines;

    [Tooltip("Bu karakter/etkileşim için benzersiz ID")]
    public string characterID;

    private int currentIndex = 0;

    void Start()
    {
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
        UpdateSpeaker(); // 👈 KİM KONUŞUYOR

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
        UpdateSpeaker(); // 👈 GÜNCELLE
    }

    void UpdateSpeaker()
    {
        if (speakerText == null)
            return;

        if (currentIndex % 2 == 0)
            speakerText.text = playerName;
        else
            speakerText.text = characterName;
    }
}
