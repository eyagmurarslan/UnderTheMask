using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
    public enum Location
    {
        Balo,
        Kutuphane,
        Balkon
    }

    [Header("Current Location")]
    public Location currentLocation;

    [Header("Location Roots")]
    public GameObject baloRoot;
    public GameObject kutuphaneRoot;
    public GameObject balkonRoot;

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

        // Mekanları kapat
        baloRoot.SetActive(false);
        kutuphaneRoot.SetActive(false);
        balkonRoot.SetActive(false);

        // Aktif mekanı aç
        switch (currentLocation)
        {
            case Location.Balo:
                baloRoot.SetActive(true);
                break;
            case Location.Kutuphane:
                kutuphaneRoot.SetActive(true);
                break;
            case Location.Balkon:
                balkonRoot.SetActive(true);
                break;
        }

        UpdateArrowButtons();

        // 🔴 Mekan değişti → açık diyalog varsa kapat
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
