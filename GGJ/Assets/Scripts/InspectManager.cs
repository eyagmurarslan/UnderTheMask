using UnityEngine;
using UnityEngine.UI;

public class InspectManager : MonoBehaviour
{
    public GameObject inspectPanel;
    public Image inspectImage;
    public Text inspectText; // PNG ALTINDAKİ YAZI

    private bool isOpen = false;

    void Start()
    {
        inspectPanel.SetActive(false);
    }

    public void Open(Sprite sprite, string description)
    {
        inspectImage.sprite = sprite;
        inspectText.text = description;

        inspectPanel.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        inspectPanel.SetActive(false);
        inspectImage.sprite = null;
        inspectText.text = "";
        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}