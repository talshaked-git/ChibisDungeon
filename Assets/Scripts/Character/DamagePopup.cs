using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
        return damagePopup;
    }

    private TMP_Text damagePopupText;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        damagePopupText = GetComponent<TMP_Text>();
    }

    public void Setup(int damgeAmount)
    {
        damagePopupText.SetText(damgeAmount.ToString());
        textColor = damagePopupText.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            Color textColor = damagePopupText.color;
            textColor.a -= disappearSpeed * Time.deltaTime;
            damagePopupText.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
