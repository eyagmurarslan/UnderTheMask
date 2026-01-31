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

    [Header("Sprites for right arrow")]
    public Sprite rightArrowDisabledSprite;
    public Sprite rightArrowEnabledSprite;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    [Header("Accuse Panel (katili tahmin et)")]
    public GameObject accusePanel; // inspector'da panel GameObject'ını assign et

    void Start()
    {
        SetLocation(Location.Balo);

        if (GameProgressTracker.Instance != null)
            GameProgressTracker.Instance.OnProgressChanged += UpdateArrowButtons;
    }

    public void SetLocation(Location newLocation)
    {
        currentLocation = newLocation;

        baloRoot.SetActive(false);
        kutuphaneRoot.SetActive(false);
        balkonRoot.SetActive(false);

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
        if (currentLocation == Location.Balkon)
        {
            if (GameProgressTracker.Instance != null && GameProgressTracker.Instance.IsAllCompleted())
            {
                if (accusePanel != null)
                {
                    accusePanel.SetActive(true);
                }
                return;
            }
        }

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
        // Sol ok: normal mantık
        leftArrow.interactable = currentLocation != Location.Balo;

        // Sağ ok: normal gezinme için interaktiflik
        bool enableRightNormally = currentLocation != Location.Balkon;

        // Eğer son lokasyondayız (Balkon), sağa geçiş yalnızca tüm hedefler tamamlandığında izin verilsin
        bool allowRightInLast = false;
        if (!enableRightNormally)
        {
            allowRightInLast = GameProgressTracker.Instance != null && GameProgressTracker.Instance.IsAllCompleted();
        }

        bool rightInteractable = enableRightNormally || allowRightInLast;
        rightArrow.interactable = rightInteractable;

        // GÖRSEL: Sprite değişimi SADECE balkondayken VE tüm hedefler tamamlandığında olsun (seçimin B'ye göre)
        bool showEnabledSprite = (currentLocation == Location.Balkon)
                                 && (GameProgressTracker.Instance != null && GameProgressTracker.Instance.IsAllCompleted());

        if (rightArrow.image != null)
        {
            if (showEnabledSprite && rightArrowEnabledSprite != null)
                rightArrow.image.sprite = rightArrowEnabledSprite;
            else if (!showEnabledSprite && rightArrowDisabledSprite != null)
                rightArrow.image.sprite = rightArrowDisabledSprite;
        }
        else
        {
            // Eğer image component yoksa hata alırsan buradan debug at
            // Debug.LogWarning("Right arrow button'da Image component bulunamadı.");
        }
    }
}