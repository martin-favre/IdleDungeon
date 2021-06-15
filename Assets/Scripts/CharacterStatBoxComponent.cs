using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PubSubSystem;

public class CharacterStatBoxComponent : MonoBehaviour
{
    public enum TargetType
    {
        Players,
        Enemies
    }
    [SerializeField]
    private TMP_Text characterName;
    [SerializeField]
    private HealthbarComponent healthbarComponent;
    [SerializeField]
    private TargetType targetType;
    [SerializeField]
    private int targetIndex;
    [SerializeField]
    private ActionButtonComponent[] actionButtonComponents;
    private int oldTargetIndex;

    Subscription<EventType> combatSubscription;
    void Start()
    {
        UpdateIndex(targetIndex, null);
        combatSubscription = CentralEventHandler.Instance.Subscribe(new[] { EventType.CombatStarted, EventType.CombatEnded }, e =>
         {
             UpdateIndex(targetIndex, e);
         });
    }

    void UpdateIndex(int index, IEvent e)
    {
        var character = GetCharacter(index);
        if (character != null)
        {
            if (characterName)
            {
                characterName.enabled = true;
                characterName.text = character.Name;
            }
            if (healthbarComponent)
            {
                healthbarComponent.gameObject.SetActive(true);
                healthbarComponent.SetAttributes(character);
            }
            foreach (var actionButton in actionButtonComponents)
            {
                actionButton.SetCharacter(character);
            }
        }
        else
        {
            if (characterName) characterName.enabled = false;
            if (healthbarComponent) healthbarComponent.gameObject.SetActive(false);
        }

        oldTargetIndex = index;
    }

    ICharacter GetCharacter(int targetIndex)
    {
        if (targetType == TargetType.Players)
        {
            var chars = SingletonProvider.MainPlayerRoster.GetAllPlayersChars();
            if (targetIndex < chars.Length)
            {
                return chars[targetIndex];
            }
            else
            {
                return null;
            }
        }
        else
        {
            var reader = SingletonProvider.MainCombatManager.CombatReader;
            if (reader != null)
            {
                var enemies = reader.GetEnemies();
                if (targetIndex < enemies.Length)
                {
                    return enemies[targetIndex];
                }
                else
                {
                    return null;
                }

            }
        }
        return null;
    }

    public void OpenUpgradeTab()
    {
        if (targetType == TargetType.Players)
        {
            UpgradePanelComponent.Instance.OpenPanel(targetIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetIndex != oldTargetIndex) // Just to make it editable from the editor
        {
            UpdateIndex(targetIndex, null);
        }
    }
}
