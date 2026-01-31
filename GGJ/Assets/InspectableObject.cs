using UnityEngine;
using UnityEngine.InputSystem;

public class InspectableObject : MonoBehaviour
{
    [Header("Visual")]
    public GameObject outlineObject;
    public Sprite inspectSprite;

    [Header("References")]
    public InspectManager inspectManager;

    private Camera cam;
    private bool isHovering;

    void Start()
    {
        cam = Camera.main;
        outlineObject.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current == null || inspectManager.IsOpen())
            return;

        Vector2 mouseWorldPos =
            cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit =
            Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        bool overThis =
            hit.collider != null && hit.collider.gameObject == gameObject;

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
            inspectManager.Open(inspectSprite);
        }
    }
}