using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // <<< bunu ekle

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

        // Eğer SpriteRenderer var ise sorting order'ı ana sprite'ınkinden 1 daha küçük yap
        var mainRenderer = GetComponent<SpriteRenderer>();
        var outlineRenderer = outlineObject.GetComponent<SpriteRenderer>();
        if (mainRenderer != null && outlineRenderer != null)
        {
            outlineRenderer.sortingLayerID = mainRenderer.sortingLayerID;
            outlineRenderer.sortingOrder = mainRenderer.sortingOrder - 1;
        }
        else
        {
            // Eğer outline UI elemanıysa, sibling index ile arkaya çek (aynı canvas içindeyse)
            var canvas = outlineObject.GetComponentInParent<Canvas>();
            if (canvas != null)
                outlineObject.transform.SetAsFirstSibling();
        }
    }

    void Update()
    {
        // Eğer fare/işaretçi şu anda bir UI öğesinin üzerindeyse sahne tıklamalarını yok say
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
            inspectManager.Open(inspectSprite);
        }
    }
}