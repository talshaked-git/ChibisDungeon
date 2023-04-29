using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeAllocateDisplayStat : MonoBehaviour
{
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;
    [SerializeField] private Button increaseStatButton;
    [SerializeField] private Button decreaseStatButton;
    [SerializeField] private AttributeAllocationPanel attributeAllocationPanel;

    private int m_statOriginalValue;
    private int m_statValue;
    private int m_statsAdded = 0;
    public int StatsAdded { get { return m_statsAdded; } }

    private void OnValidate()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);

        if (statNameText == null)
        {
            statNameText = texts[0];
        }
        if (statValueText == null)
        {
            statValueText = texts[1];
        }

        Button[] buttons = GetComponentsInChildren<Button>(true);
        if (increaseStatButton == null)
        {
            increaseStatButton = buttons[0];
        }
        if (decreaseStatButton == null)
        {
            decreaseStatButton = buttons[1];
        }

        if (attributeAllocationPanel == null)
        {
            attributeAllocationPanel = GetComponentInParent<AttributeAllocationPanel>();
        }

        increaseStatButton.onClick.AddListener(IncreaseStat);
        decreaseStatButton.onClick.AddListener(DecreaseStat);
    }

    private void IncreaseStat()
    {
        m_statsAdded++;
        m_statValue = m_statOriginalValue + m_statsAdded;
        statValueText.text = m_statValue.ToString();
        attributeAllocationPanel.DecreaseStatPoints();
    }

    private void DecreaseStat()
    {
        m_statsAdded--;
        m_statValue = m_statOriginalValue + m_statsAdded;
        statValueText.text = m_statValue.ToString();
        attributeAllocationPanel.IncreaseStatPoints();
    }

    public void Reset()
    {
        m_statValue = m_statOriginalValue;
        m_statsAdded = 0;
        statValueText.text = m_statValue.ToString();
    }

    public void ConfirmStats(Player player)
    {
        player.UseAttributePoints(m_statsAdded, statNameText.text);
        m_statsAdded = 0;
    }

    public void SetStatText(string statName, float statValue)
    {
        statNameText.text = statName;
        statValueText.text = ((int)statValue).ToString();
        m_statOriginalValue = (int)statValue;
    }

    public void DisableIncreaseButton()
    {
        increaseStatButton.interactable = false;
    }

    public void EnableIncreaseButton()
    {
        increaseStatButton.interactable = true;
    }

    public void DisableDecreaseButton()
    {
        decreaseStatButton.interactable = false;
    }

    public void EnableDecreaseButton()
    {
        decreaseStatButton.interactable = true;
    }

}
