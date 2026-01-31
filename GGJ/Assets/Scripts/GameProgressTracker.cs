using System;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressTracker : MonoBehaviour
{
    public static GameProgressTracker Instance { get; private set; }

    // Kayıtlı hedef ID'leri (karakterler + inspectable objeler)
    private HashSet<string> registeredTargets = new HashSet<string>();

    // Tamamlanan hedef ID'leri
    private HashSet<string> completedIds = new HashSet<string>();

    public event Action OnProgressChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // optional: DontDestroyOnLoad(gameObject);
    }

    // Diğer bir sistem (DialogueController/InspectableObject) sahne yüklenirken kendini kaydetsin
    public void RegisterTarget(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (registeredTargets.Add(id))
        {
            OnProgressChanged?.Invoke();
            Debug.Log($"Registered target: {id} (total targets: {registeredTargets.Count})");
        }
    }

    // (Opsiyonel) hedefi kayıttan kaldırmak istersen
    public void UnregisterTarget(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (registeredTargets.Remove(id))
        {
            // Eğer zaten tamamlanmışsa onu da temizle
            completedIds.Remove(id);
            OnProgressChanged?.Invoke();
        }
    }

    public void MarkCompleted(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (!registeredTargets.Contains(id))
        {
            // Eğer kayıtlı değilse yine de kaydedip tamamlayabilirsin veya uyarı ver
            Debug.LogWarning($"MarkCompleted called for unregistered id '{id}'. Registering automatically.");
            registeredTargets.Add(id);
        }

        if (completedIds.Add(id))
        {
            OnProgressChanged?.Invoke();
            Debug.Log($"Marked completed: {id} ({completedIds.Count}/{registeredTargets.Count})");
        }
    }

    public bool IsCompleted(string id)
    {
        return !string.IsNullOrEmpty(id) && completedIds.Contains(id);
    }

    public int CompletedCount => completedIds.Count;
    public int TotalTargets => registeredTargets.Count;

    public bool IsAllCompleted()
    {
        return registeredTargets.Count > 0 && completedIds.Count >= registeredTargets.Count;
    }

    public void ResetProgress()
    {
        registeredTargets.Clear();
        completedIds.Clear();
        OnProgressChanged?.Invoke();
    }
}