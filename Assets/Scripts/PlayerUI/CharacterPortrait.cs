using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour
{
    public Player player;
    //public Image characterImage;
    public TMP_Text characterNameText;
    public TMP_Text levelText;
    public Image hpSlider;
    public Image mpSlider;
    public Image expSlider;
    [SerializeField] private float fillSpeed = 2f;


    private void Start()
    {
        player = GameManager.instance.currentPlayer;
        
        hpSlider.fillAmount = player.currentHP / player.HP.Value;
        mpSlider.fillAmount = player.currentMP / player.MP.Value;
        expSlider.fillAmount = player.CurrentExp / player.requiredExpForNextLevel;

        hpSlider.GetComponentInChildren<TMP_Text>().text = player.currentHP.ToString() + "/" + player.HP.Value.ToString();
        mpSlider.GetComponentInChildren<TMP_Text>().text = player.currentMP.ToString() + "/" + player.MP.Value.ToString();
        expSlider.GetComponentInChildren<TMP_Text>().text = player.CurrentExp.ToString() + "/" + player.requiredExpForNextLevel.ToString();
        levelText.text = "Level " + player.Level.ToString();
        characterNameText.text = player.name;
    }

    private void Update()
    {

        if (player == null)
        {
            player = GameManager.instance.currentPlayer;
            Debug.Log("No player found");
            return;
        }

        float fillDifference = player.currentHP / player.HP.Value - hpSlider.fillAmount;
        float fillChange = Mathf.Clamp(fillDifference, -fillSpeed * Time.unscaledDeltaTime, fillSpeed * Time.unscaledDeltaTime);
        hpSlider.fillAmount += fillChange;

        fillDifference = player.currentMP / player.MP.Value - mpSlider.fillAmount;
        fillChange = Mathf.Clamp(fillDifference, -fillSpeed * Time.unscaledDeltaTime, fillSpeed * Time.unscaledDeltaTime);
        mpSlider.fillAmount += fillChange;

        fillDifference = player.CurrentExp / player.requiredExpForNextLevel - expSlider.fillAmount;
        fillChange = Mathf.Clamp(fillDifference, -fillSpeed * Time.unscaledDeltaTime, fillSpeed * Time.unscaledDeltaTime);
        expSlider.fillAmount += fillChange;

        hpSlider.GetComponentInChildren<TMP_Text>().text = player.currentHP.ToString() + "/" + player.HP.Value.ToString();
        mpSlider.GetComponentInChildren<TMP_Text>().text = player.currentMP.ToString() + "/" + player.MP.Value.ToString();
        expSlider.GetComponentInChildren<TMP_Text>().text = player.CurrentExp.ToString() + "/" + player.requiredExpForNextLevel.ToString();
        levelText.text = "Level " + player.Level.ToString();
    }
}

