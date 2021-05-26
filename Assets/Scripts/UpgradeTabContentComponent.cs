using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTabContentComponent : MonoBehaviour
{
    public enum UpgradeType
    {
        Attackiness,
        Healthiness
    }

    [Serializable]
    public struct UpgradeButton
    {
        public int level;
        public IncreaseUpgradeButton button;
    }
    [SerializeField]
    TMPro.TMP_Text nameText;
    [SerializeField]
    UpgradeType upgradeTypes;
    [SerializeField]
    UpgradeButton[] buttons;

    public void SetPlayerIndex(int index)
    {
        var character = SingletonProvider.MainPlayerRoster.GetAllPlayersChars()[index];
        if(nameText) nameText.text = character.Name;
        foreach (var button in buttons)
        {
            button.button.SetButtonType(index, button.level, upgradeTypes);
        }
    }
}
