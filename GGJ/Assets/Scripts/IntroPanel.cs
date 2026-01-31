using UnityEngine;

public class IntroPanel : MonoBehaviour
{
    public GameObject introPanel;

    public void CloseIntro()
    {
        introPanel.SetActive(false);
    }
}