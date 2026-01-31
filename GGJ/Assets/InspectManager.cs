using UnityEngine;
using UnityEngine.UI;

public class InspectManager : MonoBehaviour
{
    public GameObject inspectPanel;
    public Image inspectImage;

    private bool isOpen = false;

    void Start()
    {
        inspectPanel.SetActive(false);
    }

    public void Open(Sprite sprite)
    {
        inspectImage.sprite = sprite;
        inspectPanel.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        inspectPanel.SetActive(false);
        inspectImage.sprite = null;
        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}