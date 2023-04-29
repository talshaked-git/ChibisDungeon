using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform spacer;
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image backgroundImage;
    private RectTransform canvasRectTransform;
    private Vector2 padding = new Vector2(60, 50);
    private Color backgroundColor;
    int siblingIndex;

    private void Awake()
    {
        if (dragRectTransform == null)
            dragRectTransform = transform.parent.GetComponent<RectTransform>();
        if (backgroundImage == null)
            backgroundImage = transform.parent.GetComponent<Image>();
        if (canvas == null)
            GetCanvas();
        if (spacer == null)
            spacer = canvas.transform.Find("Spacer").GetComponent<RectTransform>();
        backgroundColor = backgroundImage.color;
        siblingIndex = spacer.GetSiblingIndex();
        canvasRectTransform = canvas.transform as RectTransform;
    }

    private void Update()
    {
        Vector2 anchoredPosition = dragRectTransform.anchoredPosition;

        if (anchoredPosition.x + dragRectTransform.rect.width + padding.x > canvasRectTransform.rect.width)
        {   // If the tooltip goes off the right side of the screen, move it to the left
            anchoredPosition.x = canvasRectTransform.rect.width - dragRectTransform.rect.width - padding.x;
        }
        if (anchoredPosition.y + dragRectTransform.rect.height + padding.y > canvasRectTransform.rect.height)
        {   // If the tooltip goes off the top side of the screen, move it to the bottom
            anchoredPosition.y = canvasRectTransform.rect.height - dragRectTransform.rect.height - padding.y;
        }

        if (anchoredPosition.x - padding.x / 2 < 0)
        {   // If the tooltip goes off the left side of the screen, move it to the right
            anchoredPosition.x = padding.x / 2;
        }
        if (anchoredPosition.y - padding.y / 2 < 0)
        {   // If the tooltip goes off the bottom side of the screen, move it to the top
            anchoredPosition.y = padding.y / 2;
        }

        dragRectTransform.anchoredPosition = anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        backgroundColor.a = 0.5f;
        backgroundImage.color = backgroundColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }


    public void OnEndDrag(PointerEventData eventData)
    {
        backgroundColor.a = 1f;
        backgroundImage.color = backgroundColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetSiblingIndex(siblingIndex - 1);
    }

    private void GetCanvas()
    {
        Transform testCanvasTransform = transform.parent;
        while (testCanvasTransform != null)
        {
            canvas = testCanvasTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                return;
            }
            testCanvasTransform = testCanvasTransform.parent;
        }
    }
}
