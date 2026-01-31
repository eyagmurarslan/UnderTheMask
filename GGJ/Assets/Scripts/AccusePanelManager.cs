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
            restartButton.onClick.AddListener(RestartScene);
            // Restart her zaman görünür/aktif olabilir; istersen başlangıçta gizle/disable da yapabilirsin
            restartButton.interactable = true;
        }

        ResetPanel();
    }

    public void OnCandidateClicked(int idx)
    {
        // Doğru seçim
        if (idx == correctIndex)
        {
            resultText.text = "Tebrikler — Katili buldun!";
            SetCandidatesInteractable(false);
            // Başarı sonrası istenirse ek işlemler yapılabilir (scene değiştir, ödül ver, vs.)
            // restart butonunu da istersen devre dışı bırakabilirsin:
            // if (restartButton != null) restartButton.interactable = false;
        }
        // Yanlış seçim -> tekrar tahmin hakkı yok, sadece restart ile yeniden başlatma
        else
        {
            resultText.text = "Yanlış kişi. Baştan oynamak için yeniden başlatın.";
            SetCandidatesInteractable(false);
            // Restart butonunu aktif bırak (zorunlu yeniden başlatma)
            if (restartButton != null)
                restartButton.interactable = true;
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
            restartButton.interactable = true; // istersen başlangıçta pasif yapabilirsin
    }

    void RestartScene()
    {
        // Eğer GameProgressTracker Singleton'ını persist (DontDestroyOnLoad) yapmışsan,
        // restart öncesi progress'i temizle.
        if (GameProgressTracker.Instance != null)
            GameProgressTracker.Instance.ResetProgress();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}