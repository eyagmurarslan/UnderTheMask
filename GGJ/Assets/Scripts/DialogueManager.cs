using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private GameObject currentPanel = null;

    public void ToggleDialogue(GameObject panelToOpen)
    {
        if (currentPanel == panelToOpen)
        {
            panelToOpen.SetActive(false);
            currentPanel = null;
            return;
        }

        if (currentPanel != null)
            currentPanel.SetActive(false);

        panelToOpen.SetActive(true);
        currentPanel = panelToOpen;
    }

    public void CloseAll()
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            currentPanel = null;
        }
    }
}