using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class HoverOutlineSimple : MonoBehaviour
{
    [Tooltip("Outline child GameObject (içinde SpriteRenderer olmalı).")]
    public GameObject outlineObject;

    private Camera cam;
    private SpriteRenderer outlineRenderer;

    void Start()
    {
        cam = Camera.main;
        if (outlineObject != null)
            outlineRenderer = outlineObject.GetComponent<SpriteRenderer>();

        // Outline objesi aktif kalsın; sadece renderer'ı kapat
        if (outlineObject != null && outlineRenderer != null)
            outlineRenderer.enabled = false;
        else if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    void Update()
    {
        // UI üstündeyse hover yoksay
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            SetOutline(false);
            return;
        }

        if (Mouse.current == null)
        {
            SetOutline(false);
            return;
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        bool overThis = hit.collider != null && hit.collider.gameObject == gameObject;
        SetOutline(overThis);
    }

    void SetOutline(bool show)
    {
        if (outlineObject == null) return;
        if (outlineRenderer != null)
            outlineRenderer.enabled = show;
        else
            outlineObject.SetActive(show);
    }
}