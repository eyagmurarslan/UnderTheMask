using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Tooltip("Oyun sahnesinin adı (Build Settings'te eklediğin sahne adı)")]
    public string sceneToLoad = "GameScene";

    [Tooltip("Credits paneli (Canvas altındaki panel). Başlangıçta inactive olmalı.")]
    public GameObject creditsPanel;

    [Tooltip("HowToPlay / Nasıl Oynanır paneli (Canvas altındaki panel). Başlangıçta inactive olmalı.")]
    public GameObject howToPlayPanel;

    // Play butonuna bağla
    public void PlayGame()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("MainMenuManager: sceneToLoad boş. Build Settings'te sahneyi ekleyip burada sahne adını yaz.");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    // Quit butonuna bağla
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Credits aç / kapat
    public void OpenCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    // HowToPlay aç / kapat
    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }
}