using UnityEngine;

public class DialogueNextButton : MonoBehaviour
{
    public void OnNextButtonClicked()
    {
        if (DialogueController.ActiveDialogue != null)
        {
            DialogueController.ActiveDialogue.NextDialogue();
        }
    }
}