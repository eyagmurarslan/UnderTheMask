using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverOutline : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public GameObject outlineObject; // 👈 ARTIK GAMEOBJECT

    void Start()
    {
        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outlineObject != null)
            outlineObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outlineObject != null)
            outlineObject.SetActive(false);
    }
}