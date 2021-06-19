using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatResultScreenComponent : MonoBehaviour
{
    [SerializeField]
    private TMP_Text goldText;
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text youGainedText;
    public void SetCombatResult(CombatResult result)
    {
        if (result.PlayerWon)
        {
            title.text = "You Still Live";
            goldText.text = result.GainedGold + " Gold coins.";
            youGainedText.text = "You Gained...";
        } else {
            title.text = "You Are Dead";
            goldText.text = "";
            youGainedText.text = "";
        }
    }
}
