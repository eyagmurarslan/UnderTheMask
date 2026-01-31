using UnityEngine;
using UnityEngine.UI;

public class LocationManagerr : MonoBehaviour
{
    public enum Location
    {
        Balo,
        Kutuphane,
        Balkon
    }

    public Location currentLocation;

    [Header("Scene Roots")]
    public GameObject baloRoot;
    public GameObject kutuphaneRoot;
    public GameObject balkonRoot;

    [Header("Canvas Roots")]
    public GameObject baloCanvasRoot;
    public GameObject kutuphaneCanvasRoot;
    public GameObject balkonCanvasRoot;

    [Header("Navigation Buttons")]
    public Button leftArrow;
    public Button rightArrow;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    void Start()
    {
        SetLocation(Location.Balo);
    }

    public void SetLocation(Location newLocation)
    {
        currentLocation = newLocation;

        // Scene root'ları kapat
        baloRoot.SetActive(false);
        kutuphaneRoot.SetActive(false);
        balkonRoot.SetActive(false);

        // Canvas root'ları kapat
        baloCanvasRoot.SetActive(false);
        kutuphaneCanvasRoot.SetActive(false);
        balkonCanvasRoot.SetActive(false);

        switch (currentLocation)
        {
            case Location.Balo:
                baloRoot.SetActive(true);
                baloCanvasRoot.SetActive(true);
                break;

            case Location.Kutuphane:
                kutuphaneRoot.SetActive(true);
                kutuphaneCanvasRoot.SetActive(true);
                break;

            case Location.Balkon:
                balkonRoot.SetActive(true);
                balkonCanvasRoot.SetActive(true);
                break;
        }

        UpdateArrowButtons();

        if (dialogueManager != null)
            dialogueManager.CloseAll();
    }

    public void GoRight()
    {
        if (currentLocation == Location.Balo)
            SetLocation(Location.Kutuphane);
        else if (currentLocation == Location.Kutuphane)
            SetLocation(Location.Balkon);
    }

    public void GoLeft()
    {
        if (currentLocation == Location.Balkon)
            SetLocation(Location.Kutuphane);
        else if (currentLocation == Location.Kutuphane)
            SetLocation(Location.Balo);
    }

    void UpdateArrowButtons()
    {
        leftArrow.interactable = currentLocation != Location.Balo;
        rightArrow.interactable = currentLocation != Location.Balkon;
    }
}
