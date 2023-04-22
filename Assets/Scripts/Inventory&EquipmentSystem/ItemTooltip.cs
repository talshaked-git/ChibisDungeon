using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemTypeText;
    [SerializeField] TMP_Text ItemDescriptionText;
    [SerializeField] TMP_Text ItemLevelText;
    [SerializeField] TMP_Text ItemClassText;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector2 tooltipSize;
    private Vector2 padding = new Vector2(40, 40); // Adjust the padding between the tooltip and the slot

    private void Awake()
    {

    }

    public void ShowTooltip(Item item, RectTransform slotRectTransform)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            tooltipSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }
        if (canvas == null)
        {
            canvas = transform.parent.GetComponent<Canvas>();
            canvasRectTransform = canvas.transform as RectTransform;
        }


        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemLevelText.text = item.EquipableLV.ToString();
        ItemClassText.text = item.EquipableClass.ToString();
        ItemDescriptionText.text = item.GetDescription();
        SetTooltipPosition(slotRectTransform);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void SetTooltipPosition(RectTransform slotRectTransform)
    {
        Vector2 newPosition = slotRectTransform.transform.position;
        Vector2 slotSize = new Vector2(slotRectTransform.rect.width, slotRectTransform.rect.height);
        if (newPosition.x + tooltipSize.x + padding.x / 2 > canvasRectTransform.rect.width)
        {   // If the tooltip goes off the right side of the screen, move it to the left
            newPosition.x = newPosition.x - tooltipSize.x - slotSize.x - padding.x * 2;
        }

        if (newPosition.y + tooltipSize.y + padding.y / 2 > canvasRectTransform.rect.height)
        {   // If the tooltip goes off the top side of the screen, move it to the bottom
            newPosition.y = newPosition.y - tooltipSize.y - slotSize.y - padding.y * 2;
        }

        if (newPosition.x - padding.x / 2 < 0)
        {   // If the tooltip goes off the left side of the screen, move it to the right
            newPosition.x = padding.x / 2;
        }

        if (newPosition.y - padding.y / 2 < 0)
        {   // If the tooltip goes off the bottom side of the screen, move it to the top
            newPosition.y = padding.y / 2;
        }




        rectTransform.position = newPosition;
    }
}
