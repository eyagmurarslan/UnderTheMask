using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccusePanelManager : MonoBehaviour
{
    [Header("UI")]
    public Button[] candidateButtons; // candidate butonları (sıralı, 0-based)
    public Text resultText;
    public Button restartButton;

    [Header("Correct answer")]
    [Tooltip("Doğru karakterin index'i candidateButtons içindeki index'e göre (0-based)")]
    public int correctIndex = 0;

    [Header("Scene")]
    [Tooltip("Ana menü sahnesinin adı (Build Settings'te eklenmiş olmalı).")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Her butona index parametresi ver ve listener ekle
        for (int i = 0; i < candidateButtons.Length; i++)
        {
            int idx = i;
            if (candidateButtons[i] != null)
            {
                candidateButtons[i].onClick.RemoveAllListeners();
                candidateButtons[i].onClick.AddListener(() => OnCandidateClicked(idx));
            }
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(ReturnToMainMenu);
            // Başlangıçta restart pasif olsun; tahmin yapıldıktan sonra aktifleşecek
            restartButton.interactable = false;
        }

        ResetPanel();
    }

    public void OnCandidateClicked(int idx)
    {
        // Tahmin yapıldığı anda butonları pasifleştir ve restart'ı aktifleştir
        SetCandidatesInteractable(false);

        // Restart butonu tahminden sonra aktif olsun
        if (restartButton != null)
            restartButton.interactable = true;

        // Doğru seçim
        if (idx == correctIndex)
        {
            resultText.text = "Tebrikler. Katili buldun!";
            // Buraya doğruysa oyunu ilerletme (scene yükle, cutscene vs) ekleyebilirsin
        }
        // Yanlış seçim
        else
        {
            resultText.text = "Yanlış kişi. Baştan oynamak için ana menüye dönün.";
        }
    }

    void SetCandidatesInteractable(bool val)
    {
        foreach (var b in candidateButtons)
            if (b != null) b.interactable = val;
    }

    public void ResetPanel()
    {
        resultText.text = "Katili tahmin et.";
        SetCandidatesInteractable(true);
        if (restartButton != null)
            restartButton.interactable = false; // Başlangıçta pasif
    }

    void ReturnToMainMenu()
    {
        // Eğer GameProgressTracker Singleton'ını persist yaptıysan, geri dönmeden önce temizle
        if (GameProgressTracker.Instance != null)
            GameProgressTracker.Instance.ResetProgress();

        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("AccusePanelManager: mainMenuSceneName boş. Build Settings'te ana menü sahne adını gir.");
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}