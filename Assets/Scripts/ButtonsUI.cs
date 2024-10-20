using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform buttonTransform;
    private Image buttonImage;

    public Color normalColor = Color.white; // Default color
    public Color hoverColor = Color.yellow; // Color on hover
    

    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>(); // Get the Image component
        buttonImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonTransform.localScale = Vector3.one * 1.05f; // Scale up
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.localScale = Vector3.one; // Reset scale
        buttonImage.color = normalColor;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonTransform.localScale = Vector3.one * 0.95f; // Scale down
        buttonImage.color = hoverColor;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonTransform.localScale = Vector3.one * 1.05f; // Scale up
    }
}
