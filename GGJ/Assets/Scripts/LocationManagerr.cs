using UnityEngine;
using UnityEngine.UI;

public class LocationManagerr : MonoBehaviour
{
    public enum Location
    {
        Balo,
        Koridor,   // yeni eklendi — Balo'dan sonra geliyor
        Kutuphane,
        Balkon
    }

    public Location currentLocation;

    [Header("Scene Roots")]
    public GameObject baloRoot;
    public GameObject koridorRoot;     // yeni
    public GameObject kutuphaneRoot;
    public GameObject balkonRoot;

    [Header("Canvas Roots")]
    public GameObject baloCanvasRoot;
    public GameObject koridorCanvasRoot; // yeni
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

        // Tüm root'ları kapat
        baloRoot.SetActive(false);
        koridorRoot.SetActive(false);
        kutuphaneRoot.SetActive(false);
        balkonRoot.SetActive(false);

        baloCanvasRoot.SetActive(false);
        koridorCanvasRoot.SetActive(false);
        kutuphaneCanvasRoot.SetActive(false);
        balkonCanvasRoot.SetActive(false);

        // Yeni lokasyonu aç
        switch (currentLocation)
        {
            case Location.Balo:
                baloRoot.SetActive(true);
                baloCanvasRoot.SetActive(true);
                break;

            case Location.Koridor:
                koridorRoot.SetActive(true);
                koridorCanvasRoot.SetActive(true);
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
        // Eğer son lokasyondaysak accuse panel kontrolü (aynı mantık)
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

        // Normal geçiş sırası: Balo -> Koridor -> Kutuphane -> Balkon
        if (currentLocation == Location.Balo)
            SetLocation(Location.Koridor);
        else if (currentLocation == Location.Koridor)
            SetLocation(Location.Kutuphane);
        else if (currentLocation == Location.Kutuphane)
            SetLocation(Location.Balkon);
    }

    public void GoLeft()
    {
        // Ters geçiş: Balkon -> Kutuphane -> Koridor -> Balo
        if (currentLocation == Location.Balkon)
            SetLocation(Location.Kutuphane);
        else if (currentLocation == Location.Kutuphane)
            SetLocation(Location.Koridor);
        else if (currentLocation == Location.Koridor)
            SetLocation(Location.Balo);
    }

    void UpdateArrowButtons()
    {
        // Sol ok: Balo ilk lokasyon olduğundan oradaysa false
        leftArrow.interactable = currentLocation != Location.Balo;

        // Sağ ok: normal gezinme için interaktiflik (son lokasyon = Balkon)
        bool enableRightNormally = currentLocation != Location.Balkon;

        // Eğer son lokasyondayız (Balkon), sağa geçiş yalnızca tüm hedefler tamamlandığında izin verilsin
        bool allowRightInLast = false;
        if (!enableRightNormally)
        {
            allowRightInLast = GameProgressTracker.Instance != null && GameProgressTracker.Instance.IsAllCompleted();
        }

        bool rightInteractable = enableRightNormally || allowRightInLast;
        rightArrow.interactable = rightInteractable;

        // GÖRSEL: Sprite değişimi SADECE balkondayken VE tüm hedefler tamamlandığında olsun
        bool showEnabledSprite = (currentLocation == Location.Balkon)
                                 && (GameProgressTracker.Instance != null && GameProgressTracker.Instance.IsAllCompleted());

        if (rightArrow.image != null)
        {
            if (showEnabledSprite && rightArrowEnabledSprite != null)
                rightArrow.image.sprite = rightArrowEnabledSprite;
            else if (!showEnabledSprite && rightArrowDisabledSprite != null)
                rightArrow.image.sprite = rightArrowDisabledSprite;
        }
    }
}