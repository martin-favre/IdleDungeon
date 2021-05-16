using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthbarComponent : MonoBehaviour
{

    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private TMP_Text text;

    public void SetAttributes(ICharacter character)
    {
        UpdateBarFill(character.Attributes);
        UpdateText(character.Attributes);
    }

    void UpdateBarFill(ICombatAttributes attributes)
    {
        var amount = attributes.CurrentHp / attributes.MaxHp;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
        hpBar.fillAmount = (float)amount;
        
    }

    void UpdateText(ICombatAttributes attributes)
    {
        if (!text) return;
        text.text = Mathf.RoundToInt((float)attributes.CurrentHp) + "/" + Mathf.RoundToInt((float)attributes.MaxHp);
    }

    void Update()
    {

    }
}
