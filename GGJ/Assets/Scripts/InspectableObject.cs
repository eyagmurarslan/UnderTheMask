using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InspectableObject : MonoBehaviour
{
    [Header("Visual")]
    public GameObject outlineObject;
    public Sprite inspectSprite;

    [Header("References")]
    public InspectManager inspectManager;

    [Header("Inspect Text")]
    [TextArea(2, 5)]
    public string inspectDescription;

    [Header("Progress")]
    [Tooltip("Bu obje için benzersiz ID (ör: 'obj_01'). Boşsa progress'e dahil edilmez.")]
    public string interactionID;

    private Camera cam;
    private bool isHovering;

    void Start()
    {
        cam = Camera.main;
        outlineObject.SetActive(false);

        if (!string.IsNullOrEmpty(interactionID) && GameProgressTracker.Instance != null)
        {
            GameProgressTracker.Instance.RegisterTarget(interactionID);
        }
    }

    void Update()
    {
        // UI üstündeyken sahne tıklamalarını iptal et
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Mouse.current == null || inspectManager.IsOpen())
            return;

        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        bool overThis = hit.collider != null && hit.collider.gameObject == gameObject;

        // Hover
        if (overThis && !isHovering)
        {
            isHovering = true;
            outlineObject.SetActive(true);
        }
        else if (!overThis && isHovering)
        {
            isHovering = false;
            outlineObject.SetActive(false);
        }

        // Click
        if (overThis && Mouse.current.leftButton.wasPressedThisFrame)
        {
            inspectManager.Open(inspectSprite, inspectDescription);

            if (!string.IsNullOrEmpty(interactionID) && GameProgressTracker.Instance != null)
            {
                GameProgressTracker.Instance.MarkCompleted(interactionID);
            }
        }
    }
}
